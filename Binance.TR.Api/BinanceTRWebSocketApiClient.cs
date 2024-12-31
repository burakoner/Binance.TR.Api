namespace Binance.TR.Api;

/// <summary>
/// BitMart WebSocket Client
/// </summary>
public class BinanceTRWebSocketApiClient : WebSocketApiClient
{
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
        KeepAliveInterval = TimeSpan.Zero;

        //IgnoreHandlingList = ["pong"];
        //SendPeriodic("PING", TimeSpan.FromSeconds(5), conn => "ping");
    }

    #region Public Methods
    /// <summary>
    /// Set API Credentials
    /// </summary>
    /// <param name="key">API Key</param>
    /// <param name="secret">API Secret</param>
    public void SetApiCredentials(string key, string secret)
        => base.SetApiCredentials(new ApiCredentials(key, secret));
    #endregion

    #region Overrided Methods
    protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
        => new BinanceTRAuthenticationProvider(credentials);

    protected override async Task<CallResult<bool>> AuthenticateAsync(WebSocketConnection connection)
    {
        var result = false;

        /*
        if (this.AuthenticationProvider == null)
            return new CallResult<bool>(new NoApiCredentialsError());

        var timestamp = DateTime.UtcNow.ConvertToMilliseconds();
        var key = this.AuthenticationProvider.Credentials.Key!.GetString();
        var signtext = $"{timestamp}#{memo}#bitmart.WebSocket";
        var signature = ((BitMartAuthenticationProvider)this.AuthenticationProvider).StreamApiSignature(signtext);

        var request = new BitMartWebSocketRequest();

        // Spot WebSocket
        if (connection.Tag.Contains("ws-manager-compress.bitmart.com"))
        {
            request.Operation = "login";
            request.Parameters = [key, timestamp.ToString(), signature];
        }

        // Futures WebSocket
        else if (connection.Tag.Contains("openapi-ws-v2.bitmart.com"))
        {
            request.Action = "access";
            request.Parameters = [key, timestamp.ToString(), signature, "web"];
        }

        // JSON
        var json = JsonConvert.SerializeObject(request);

        var result = false;
        await connection.SendAndWaitAsync(request, ClientOptions.ResponseTimeout, data =>
        {
            // Check Point
            if (data.Type != JTokenType.Object)
                return false;

            // Spot WebSocket
            if (data["event"] != null)
                result = (string)data["event"] == "login";

            // Futures WebSocket
            if (data["action"] != null && data["success"] != null)
                result = (string)data["action"] == "access" && (bool)data["success"];

            // Return
            return result;
        });
        */

        return result
            ? new CallResult<bool>(result)
            : new CallResult<bool>(new ServerError("Unspecified Error"));
    }

    protected override bool HandleQueryResponse<T>(WebSocketConnection connection, object request, JToken data, out CallResult<T> callResult)
    {
        // Call Result
        callResult = null;

        /*
        // Ping Request
        if (request.ToString() == "ping" && data.ToString() == "pong")
            return true;
        */

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
            // Trades
            if ((string)data["e"] == "trade")
            {
                return bRequest.Parameters.Any(x => x.Contains((string)data["s"] + "@trade", StringComparison.InvariantCultureIgnoreCase));
            }

            // Aggregated Trades
            if ((string)data["e"] == "aggTrade")
            {
                return bRequest.Parameters.Any(x => x.Contains((string)data["s"] + "@aggTrade", StringComparison.InvariantCultureIgnoreCase));
            }

            // Klines
            if ((string)data["e"] == "kline")
            {
                return bRequest.Parameters.Any(x => x.Contains((string)data["s"] + "@kline_", StringComparison.InvariantCultureIgnoreCase));
            }

            // Symbol Tickers
            if ((string)data["e"] == "24hrMiniTicker")
            {
                return bRequest.Parameters.Any(x => x.Contains((string)data["s"] + "@miniTicker", StringComparison.InvariantCultureIgnoreCase));
            }

            // Depth Diff
            if ((string)data["e"] == "depthUpdate")
            {
                return bRequest.Parameters.Any(x => x.Contains((string)data["s"] + "@depth", StringComparison.InvariantCultureIgnoreCase));
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
        var request = new BinanceTRSocketRequest();
        var result = false;

        /*
        // Spot WebSocket
        if (connection.Tag.Contains("ws-manager-compress.bitmart.com"))
        {
            request.Operation = "unsubscribe";
            request.Parameters = bRequest.Parameters;
        }

        // Futures WebSocket
        else if (connection.Tag.Contains("openapi-ws-v2.bitmart.com"))
        {
            request.Action = "access";
            request.Parameters = bRequest.Parameters;
        }

        // JSON
        var json = JsonConvert.SerializeObject(request);

        var result = false;
        await connection.SendAndWaitAsync(request, ClientOptions.ResponseTimeout, data =>
        {
            // Check Point
            if (data.Type != JTokenType.Object)
                return false;

            // Spot WebSocket
            if (data["event"] != null && data["topic"] != null)
            {
                var evt = (string)data["event"];
                var topic = (string)data["topic"];

                return evt == "unsubscribe" && bRequest.Parameters.Contains(topic);
            }

            // Futures WebSocket
            if (data["action"] != null && data["group"] != null && data["success"] != null)
            {
                var act = (string)data["action"];
                var group = (string)data["group"];
                var success = (bool)data["success"];

                return act == "subscribe" && bRequest.Parameters.Contains(group) && success;
            }

            // Return
            return false;
        });
        */

        return result;
    }
    #endregion

    #region Internal Methods
    internal string BuildUrl(BinanceDataCenter datacenter, params string[] parameters)
    {
        var baseAddress = "";
        if (datacenter == BinanceDataCenter.WebSocketMain) baseAddress = BinanceTRAddress.Default.WebSocketMainAddress;
        if (datacenter == BinanceDataCenter.WebSocketNext) baseAddress = BinanceTRAddress.Default.WebSocketNextAddress;

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
        return await SubscribeAsync(BuildUrl(BinanceDataCenter.WebSocketMain), new BinanceTRSocketRequest
        {
            Id = id,
            Method = "SUBSCRIBE",
            Parameters = symbols.Select(x => $"{(x.Replace("-", "").Replace("_", "").ToLower())}@trade").ToArray()
        }, id.ToString(), false, internalHandler, ct).ConfigureAwait(false);
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
        return await SubscribeAsync(BuildUrl(BinanceDataCenter.WebSocketMain), new BinanceTRSocketRequest
        {
            Id = id,
            Method = "SUBSCRIBE",
            Parameters = symbols.Select(x => $"{(x.Replace("-", "").Replace("_", "").ToLower())}@aggTrade").ToArray()
        }, id.ToString(), false, internalHandler, ct).ConfigureAwait(false);
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
        return await SubscribeAsync(BuildUrl(BinanceDataCenter.WebSocketMain), new BinanceTRSocketRequest
        {
            Id = id,
            Method = "SUBSCRIBE",
            Parameters = symbols.Select(x => $"{(x.Replace("-", "").Replace("_", "").ToLower())}@kline_{MapConverter.GetString(interval)}").ToArray()
        }, id.ToString(), false, internalHandler, ct).ConfigureAwait(false);
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
        return await SubscribeAsync(BuildUrl(BinanceDataCenter.WebSocketMain), new BinanceTRSocketRequest
        {
            Id = id,
            Method = "SUBSCRIBE",
            Parameters = symbols.Select(x => $"{(x.Replace("-", "").Replace("_", "").ToLower())}@miniTicker").ToArray()
        }, id.ToString(), false, internalHandler, ct).ConfigureAwait(false);
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
        return await SubscribeAsync(BuildUrl(BinanceDataCenter.WebSocketMain), new BinanceTRSocketRequest
        {
            Id = id,
            Method = "SUBSCRIBE",
            Parameters = ["!miniTicker@arr"]
        }, id.ToString(), false, internalHandler, ct).ConfigureAwait(false);
    }

    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToPartialDepthAsync(string symbol, int level, int speed, Action<BinanceTRStreamOrderBook> handler, CancellationToken ct = default)
        => await SubscribeToPartialDepthAsync([symbol], level, speed, handler, ct).ConfigureAwait(false);
    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToPartialDepthAsync(IEnumerable<string> symbols, int level, int speed, Action<BinanceTRStreamOrderBook> handler, CancellationToken ct = default)
    {
        // Validations
        level.ValidateIntValues(nameof(level), 5, 10, 20);
        speed.ValidateIntValues(nameof(speed), 100, 1000);

        // Internal Handler
        var internalHandler = new Action<WebSocketDataEvent<BinanceTRStreamOrderBook>>(data =>
        {
            if (data?.Data != null) handler(data.Data);
        });

        // Identifier
        var id = NextId();

        // Subscribe
        return await SubscribeAsync(BuildUrl(BinanceDataCenter.WebSocketMain), new BinanceTRSocketRequest
        {
            Id = id,
            Method = "SUBSCRIBE",
            Parameters = symbols.Select(x => $"{(x.Replace("-", "").Replace("_", "").ToLower())}@depth{level}@{speed}ms").ToArray()
        }, id.ToString(), false, internalHandler, ct).ConfigureAwait(false);
    }

    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToDepthDiffAsync(string symbol, int speed, Action<BinanceTRStreamOrderBookUpdate> handler, CancellationToken ct = default)
        => await SubscribeToDepthDiffAsync([symbol], speed, handler, ct).ConfigureAwait(false);
    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToDepthDiffAsync(IEnumerable<string> symbols, int speed, Action<BinanceTRStreamOrderBookUpdate> handler, CancellationToken ct = default)
    {
        // Validations
        speed.ValidateIntValues(nameof(speed), 100, 1000);

        // Internal Handler
        var internalHandler = new Action<WebSocketDataEvent<BinanceTRStreamOrderBookUpdate>>(data =>
        {
            if (data?.Data != null) handler(data.Data);
        });

        // Identifier
        var id = NextId();

        // Subscribe
        return await SubscribeAsync(BuildUrl(BinanceDataCenter.WebSocketMain), new BinanceTRSocketRequest
        {
            Id = id,
            Method = "SUBSCRIBE",
            Parameters = symbols.Select(x => $"{(x.Replace("-", "").Replace("_", "").ToLower())}@depth@{speed}ms").ToArray()
        }, id.ToString(), false, internalHandler, ct).ConfigureAwait(false);
    }
}
