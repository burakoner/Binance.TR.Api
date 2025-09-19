namespace Binance.TR.Api;

/// <summary>
/// BitMart WebSocket Client
/// </summary>
public class BinanceTRWebSocketApiClient : WebSocketApiClient
{
    /// <summary>
    /// ConnectionLost Event
    /// </summary>
    public event EventHandler ConnectionLost;

    /// <summary>
    /// Use to override WebSocket Main Address
    /// </summary>
    public string WebSocketMainAddress { get; set; } = string.Empty;

    /// <summary>
    /// Use to override WebSocket Next Address
    /// </summary>
    public string WebSocketNextAddress { get; set; } = string.Empty;

    internal CultureInfo CI { get => CultureInfo.InvariantCulture; }

    /// <summary>
    /// Creates a new instance of BitMartWebSocketClient
    /// </summary>
    public BinanceTRWebSocketApiClient() : this(new BinanceTRWebSocketApiOptions(), null)
    {
    }

    /// <summary>
    /// Creates a new instance of BinanceTRWebSocketApiClient
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="options"></param>
    public BinanceTRWebSocketApiClient(BinanceTRWebSocketApiOptions options, ILogger logger = null) : base(logger, options)
    {
        ContinueOnQueryResponse = true;
        UnhandledMessageExpected = true;
        KeepAliveInterval = TimeSpan.FromMinutes(1);

        //IgnoreHandlingList = ["pong"];
        //SendPeriodic("PING", TimeSpan.FromSeconds(5), conn => "ping");
    }

    #region Public Methods
    /// <summary>
    /// Set API Credentials
    /// </summary>
    /// <param name="key">API Key</param>
    /// <param name="secret">API Secret</param>
    public void SetApiCredentials(string key, string secret) => base.SetApiCredentials(new ApiCredentials(key, secret));
    #endregion

    #region Overrided Methods
    protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials) => new BinanceTRAuthenticationProvider(credentials);

    protected override Task<CallResult<bool>> AuthenticateAsync(WebSocketConnection connection)
    {
        throw new NotImplementedException();
    }

    protected override bool HandleQueryResponse<T>(WebSocketConnection connection, object request, JToken data, out CallResult<T> callResult)
    {
        throw new NotImplementedException();
    }

    protected override bool HandleSubscriptionResponse(WebSocketConnection connection, WebSocketSubscription subscription, object request, JToken data, out CallResult<object> callResult)
    {
        callResult = null;
        if (data.Type != JTokenType.Object)
            return false;

        var bRequest = (BinanceTRSocketRequest)request;
        if (data["id"] != null && data["result"] != null)
        {
            var success = (int)data["id"] == bRequest.Id;
            if (success) callResult = new CallResult<object>(true);
            return success;
        }
        else
        {
            callResult = new CallResult<object>(new UnknownError("Unknown Error"));
        }

        return false;
    }

    protected override bool MessageMatchesHandler(WebSocketConnection connection, JToken data, object request)
    {
        // Ping Request
        //if (request.ToString() == "ping" && data.ToString() == "pong")
        //    return true;

        // Subscription Response
        if (data.Type == JTokenType.Object && data["id"] != null && data["result"] != null)
            return false;

        // Spot WebSocket
        if (request is not BinanceTRSocketRequest bRequest)
            return false;

        // Object Responses
        if (data.Type == JTokenType.Object && data["e"] != null && data["s"] != null)
        {
            // Event Type
            var eventType = (string)data["e"];

            // Trades
            if (eventType == "trade")
            {
                return bRequest.Parameters.Any(x => x.Contains((string)data["s"] + "@trade", StringComparison.InvariantCultureIgnoreCase));
            }

            // Aggregated Trades
            if (eventType == "aggTrade")
            {
                return bRequest.Parameters.Any(x => x.Contains((string)data["s"] + "@aggTrade", StringComparison.InvariantCultureIgnoreCase));
            }

            // Klines
            if (eventType == "kline")
            {
                return bRequest.Parameters.Any(x => x.Contains((string)data["s"] + "@kline_", StringComparison.InvariantCultureIgnoreCase));
            }

            // Symbol Tickers
            if (eventType == "24hrMiniTicker")
            {
                return bRequest.Parameters.Any(x => x.Contains((string)data["s"] + "@miniTicker", StringComparison.InvariantCultureIgnoreCase));
            }

            // Depth Diff
            if (eventType == "depthUpdate")
            {
                return bRequest.Parameters.Any(x => x.Contains((string)data["s"] + "@depth", StringComparison.InvariantCultureIgnoreCase));
            }

        }

        // Object Responses
        if (data.Type == JTokenType.Object && data["e"] != null)
        {
            // Event Type
            var eventType = (string)data["e"];

            // User Stream
            if (eventType == "balanceUpdate" || eventType == "outboundAccountPosition" || eventType == "executionReport")
            {
                return bRequest.Parameters.Any(x => x.Length == 60 && !x.Contains("@"));
            }
        }

        // Object Responses
        if (data.Type == JTokenType.Object)
        {
            // Partial Depth
            if (data["lastUpdateId"] != null)
            {
                return bRequest.Parameters.Any(x => x.Contains("@depth", StringComparison.InvariantCultureIgnoreCase));
            }
        }

        // Array Responses
        if (data.Type == JTokenType.Array && data.Count() > 0)
        {
            // All Tickers
            if ((string)data[0]["e"] == "24hrMiniTicker")
            {
                return bRequest.Parameters.Any(x => x == "!miniTicker@arr");
            }
        }

        // Return
        return false;
    }

    protected override bool MessageMatchesHandler(WebSocketConnection connection, JToken message, string identifier)
    {
        throw new NotImplementedException();
    }

    protected override async Task<bool> UnsubscribeAsync(WebSocketConnection connection, WebSocketSubscription subscription)
    {
        var bRequest = ((BinanceTRSocketRequest)subscription.Request!);
        var request = new BinanceTRSocketRequest
        {
            Id = NextId(),
            Method = "UNSUBSCRIBE",
            Parameters = bRequest.Parameters,
        };

        // Send Request
        await connection.SendAndWaitAsync(request, ClientOptions.ResponseTimeout, data =>
        {
            // Check Point
            if (data.Type != JTokenType.Object)
                return false;

            // Check Point
            if (data["id"] != null && data["result"] != null)
            {
                return (int)data["id"] == request.Id;
            }

            // Return
            return false;
        });

        return false;
    }
    #endregion

    #region Internal Methods
    internal string BuildUrl(BinanceDataCenter datacenter, params string[] parameters)
    {
        var baseAddress = "";
        if (datacenter == BinanceDataCenter.WebSocketMain) baseAddress = string.IsNullOrEmpty(WebSocketMainAddress) ? BinanceTRAddress.Default.WebSocketMainAddress : WebSocketMainAddress;
        if (datacenter == BinanceDataCenter.WebSocketNext) baseAddress = string.IsNullOrEmpty(WebSocketNextAddress) ? BinanceTRAddress.Default.WebSocketNextAddress : WebSocketNextAddress;

        return $"{baseAddress.TrimEnd('/')}/{string.Join("/", parameters).TrimStart('/')}".Trim('/');
    }
    #endregion

    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTradesAsync(string symbol, Action<BinanceTRStreamTrade> handler, CancellationToken ct = default)
        => await SubscribeToTradesAsync([symbol], handler, ct).ConfigureAwait(false);
    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTradesAsync(IEnumerable<string> symbols, Action<BinanceTRStreamTrade> handler, CancellationToken ct = default)
    {
        // Internal Handler
        var internalHandler = new Action<WebSocketDataEvent<BinanceTRStreamTrade>>(data =>
        {
            if (data?.Data != null) handler(data.Data);
        });

        // Identifier
        var id = NextId();

        // Subscribe
        var subscription = await SubscribeAsync(BuildUrl(BinanceDataCenter.WebSocketMain), new BinanceTRSocketRequest
        {
            Id = id,
            Method = "SUBSCRIBE",
            Parameters = symbols.Select(x => $"{(x.Replace("-", "").Replace("_", "").ToLower(CI))}@trade").ToArray()
        }, id.ToString(), false, internalHandler, ct).ConfigureAwait(false);

        // Connection Lost Event
        if (ClientOptions.AutoReconnect)
            subscription.Data.ConnectionLost += DataOnConnectionLost;

        // Return
        return subscription;
    }

    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAggregatedTradesAsync(string symbol, Action<BinanceTRStreamAggregatedTrade> handler, CancellationToken ct = default)
        => await SubscribeToAggregatedTradesAsync([symbol], handler, ct).ConfigureAwait(false);
    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAggregatedTradesAsync(IEnumerable<string> symbols, Action<BinanceTRStreamAggregatedTrade> handler, CancellationToken ct = default)
    {
        // Internal Handler
        var internalHandler = new Action<WebSocketDataEvent<BinanceTRStreamAggregatedTrade>>(data =>
        {
            if (data?.Data != null) handler(data.Data);
        });

        // Identifier
        var id = NextId();

        // Subscribe
        var subscription = await SubscribeAsync(BuildUrl(BinanceDataCenter.WebSocketMain), new BinanceTRSocketRequest
        {
            Id = id,
            Method = "SUBSCRIBE",
            Parameters = symbols.Select(x => $"{(x.Replace("-", "").Replace("_", "").ToLower(CI))}@aggTrade").ToArray()
        }, id.ToString(), false, internalHandler, ct).ConfigureAwait(false);

        // Connection Lost Event
        if (ClientOptions.AutoReconnect)
            subscription.Data.ConnectionLost += DataOnConnectionLost;

        // Return
        return subscription;
    }

    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToKlinesAsync(string symbol, KlineInterval interval, Action<BinanceTRStreamKline> handler, CancellationToken ct = default)
        => await SubscribeToKlinesAsync([symbol], interval, handler, ct).ConfigureAwait(false);
    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToKlinesAsync(IEnumerable<string> symbols, KlineInterval interval, Action<BinanceTRStreamKline> handler, CancellationToken ct = default)
    {
        // Internal Handler
        var internalHandler = new Action<WebSocketDataEvent<BinanceTRStreamKlineContainer>>(data =>
        {
            if (data?.Data?.Kline != null) handler(data.Data.Kline);
        });

        // Identifier
        var id = NextId();

        // Subscribe
        var subscription = await SubscribeAsync(BuildUrl(BinanceDataCenter.WebSocketMain), new BinanceTRSocketRequest
        {
            Id = id,
            Method = "SUBSCRIBE",
            Parameters = symbols.Select(x => $"{(x.Replace("-", "").Replace("_", "").ToLower(CI))}@kline_{MapConverter.GetString(interval)}").ToArray()
        }, id.ToString(), false, internalHandler, ct).ConfigureAwait(false);

        // Connection Lost Event
        if (ClientOptions.AutoReconnect)
            subscription.Data.ConnectionLost += DataOnConnectionLost;

        // Return
        return subscription;
    }

    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTickersAsync(string symbol, Action<BinanceTRStreamTicker> handler, CancellationToken ct = default)
        => await SubscribeToTickersAsync([symbol], handler, ct).ConfigureAwait(false);
    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTickersAsync(IEnumerable<string> symbols, Action<BinanceTRStreamTicker> handler, CancellationToken ct = default)
    {
        // Internal Handler
        var internalHandler = new Action<WebSocketDataEvent<BinanceTRStreamTicker>>(data =>
        {
            if (data?.Data != null) handler(data.Data);
        });

        // Identifier
        var id = NextId();

        // Subscribe
        var subscription = await SubscribeAsync(BuildUrl(BinanceDataCenter.WebSocketMain), new BinanceTRSocketRequest
        {
            Id = id,
            Method = "SUBSCRIBE",
            Parameters = symbols.Select(x => $"{(x.Replace("-", "").Replace("_", "").ToLower(CI))}@miniTicker").ToArray()
        }, id.ToString(), false, internalHandler, ct).ConfigureAwait(false);

        // Connection Lost Event
        if (ClientOptions.AutoReconnect)
            subscription.Data.ConnectionLost += DataOnConnectionLost;

        // Return
        return subscription;
    }
    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTickersAsync(Action<BinanceTRStreamTicker> handler, CancellationToken ct = default)
    {
        // Internal Handler
        var internalHandler = new Action<WebSocketDataEvent<List<BinanceTRStreamTicker>>>(data =>
        {
            if (data?.Data != null)
            {
                foreach (var ticker in data.Data)
                    handler(ticker);
            }
        });

        // Identifier
        var id = NextId();

        // Subscribe
        var subscription = await SubscribeAsync(BuildUrl(BinanceDataCenter.WebSocketMain), new BinanceTRSocketRequest
        {
            Id = id,
            Method = "SUBSCRIBE",
            Parameters = ["!miniTicker@arr"]
        }, id.ToString(), false, internalHandler, ct).ConfigureAwait(false);

        // Connection Lost Event
        if (ClientOptions.AutoReconnect)
            subscription.Data.ConnectionLost += DataOnConnectionLost;

        // Return
        return subscription;
    }

    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToPartialDepthAsync(string symbol, int level, int speed, Action<BinanceTRStreamOrderBookPartial> handler, CancellationToken ct = default)
        => await SubscribeToPartialDepthAsync([symbol], level, speed, handler, ct).ConfigureAwait(false);
    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToPartialDepthAsync(IEnumerable<string> symbols, int level, int speed, Action<BinanceTRStreamOrderBookPartial> handler, CancellationToken ct = default)
    {
        // Validations
        level.ValidateIntValues(nameof(level), 5, 10, 20);
        speed.ValidateIntValues(nameof(speed), 100, 1000);

        // Internal Handler
        var internalHandler = new Action<WebSocketDataEvent<BinanceTRStreamOrderBookPartial>>(data =>
        {
            if (data?.Data != null) handler(data.Data);
        });

        // Identifier
        var id = NextId();

        // Subscribe
        var subscription = await SubscribeAsync(BuildUrl(BinanceDataCenter.WebSocketMain), new BinanceTRSocketRequest
        {
            Id = id,
            Method = "SUBSCRIBE",
            Parameters = symbols.Select(x => $"{(x.Replace("-", "").Replace("_", "").ToLower(CI))}@depth{level}@{speed}ms").ToArray()
        }, id.ToString(), false, internalHandler, ct).ConfigureAwait(false);

        // Connection Lost Event
        if (ClientOptions.AutoReconnect)
            subscription.Data.ConnectionLost += DataOnConnectionLost;

        // Return
        return subscription;
    }

    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToDepthDiffAsync(string symbol, int speed, Action<BinanceTRStreamOrderBookDiff> handler, CancellationToken ct = default)
        => await SubscribeToDepthDiffAsync([symbol], speed, handler, ct).ConfigureAwait(false);
    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToDepthDiffAsync(IEnumerable<string> symbols, int speed, Action<BinanceTRStreamOrderBookDiff> handler, CancellationToken ct = default)
    {
        // Validations
        speed.ValidateIntValues(nameof(speed), 100, 1000);

        // Internal Handler
        var internalHandler = new Action<WebSocketDataEvent<BinanceTRStreamOrderBookDiff>>(data =>
        {
            if (data?.Data != null) handler(data.Data);
        });

        // Identifier
        var id = NextId();

        // Subscribe
        var subscription = await SubscribeAsync(BuildUrl(BinanceDataCenter.WebSocketMain), new BinanceTRSocketRequest
        {
            Id = id,
            Method = "SUBSCRIBE",
            Parameters = symbols.Select(x => $"{(x.Replace("-", "").Replace("_", "").ToLower(CI))}@depth@{speed}ms").ToArray()
        }, id.ToString(), false, internalHandler, ct).ConfigureAwait(false);

        // Connection Lost Event
        if (ClientOptions.AutoReconnect)
            subscription.Data.ConnectionLost += DataOnConnectionLost;

        // Return
        return subscription;
    }

    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToUserStreamAsync(
        string listenKey,
        Action<BinanceTRStreamAccountUpdateBalance> onBalanceUpdate,
        Action<BinanceTRStreamOrderUpdate> onOrderUpdate,
        CancellationToken ct = default)
    {
        // Internal Handler
        // var internalHandler = new Action<WebSocketDataEvent<BinanceTRStreamEvent>>(data =>
        // Internal Handler using JToken instead of WebSocketDataEvent to avoid data.Raw null issue
        var internalHandler = new Action<WebSocketDataEvent<JToken>>(data =>
        {
            if (data?.Data == null) return;
            var eventType = data.Data["e"]?.ToString();
            if (string.IsNullOrEmpty(eventType)) return;

            if (eventType == "outboundAccountPosition")
            {
                // Sample: {"e":"outboundAccountPosition","E":1735761723087,"u":1735761723087,"B":[{"a":"USDT","f":"34.62550600","l":"0.00000000"}]}
                var accountUpdate = data.Data.ToObject<BinanceTRStreamAccountUpdate>();
                if (accountUpdate != null) foreach (var balance in accountUpdate.Balances) onBalanceUpdate(balance);
            }
            else if (eventType == "balanceUpdate")
            {
                // Sample: {"e":"balanceUpdate","E":1735760578674,"a":"USDT","d":"-0.01000000","T":1735760578674}
                return;
            }
            else if (eventType == "executionReport")
            {
                // Sample: {"e":"executionReport","E":1735762034132,"s":"BTCUSDT","c":"911608173","S":"BUY","o":"LIMIT","f":"GTC","q":"0.00030000","p":"90000.00000000","P":"0.00000000","F":"0.00000000","g":-1,"C":"","x":"NEW","X":"NEW","r":"NONE","i":34599337295,"l":"0.00000000","z":"0.00000000","L":"0.00000000","n":"0","N":null,"T":1735762034131,"t":-1,"I":73945302308,"w":true,"m":false,"M":false,"O":1735762034131,"Z":"0.00000000","Y":"0.00000000","Q":"0.00000000","W":1735762034131,"V":"EXPIRE_MAKER"}
                var orderUpdate = data.Data.ToObject<BinanceTRStreamOrderUpdate>();
                if (orderUpdate != null) onOrderUpdate(orderUpdate);
            }
        });

        // Identifier
        var id = NextId();

        // Subscribe
        var subscription = await SubscribeAsync(BuildUrl(BinanceDataCenter.WebSocketMain), new BinanceTRSocketRequest
        {
            Id = id,
            Method = "SUBSCRIBE",
            Parameters = [listenKey]
        }, id.ToString(), false, internalHandler, ct).ConfigureAwait(false);

        // Connection Lost Event
        if (ClientOptions.AutoReconnect)
            subscription.Data.ConnectionLost += DataOnConnectionLost;

        // Return
        return subscription;
    }

    private void DataOnConnectionLost()
    {
        ConnectionLost?.Invoke(this, EventArgs.Empty);
    }
}
