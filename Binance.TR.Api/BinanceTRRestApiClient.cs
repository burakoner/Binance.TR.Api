namespace Binance.TR.Api;

/// <summary>
/// Binance TR Rest API Client
/// </summary>
public class BinanceTRRestApiClient : RestApiClient
{
    /// <summary>
    /// Use to override Default Rest Api Address
    /// </summary>
    public string MeRestApiAddress { get; set; } = string.Empty;

    /// <summary>
    /// Use to override Default Rest Api Address
    /// </summary>
    public string TrRestApiAddress { get; set; } = string.Empty;

    /// <summary>
    /// Use to override Default 2meta Rest Api Address
    /// </summary>
    public string AppRestApiAddress { get; set; } = string.Empty;

    internal BinanceTRRestApiOptions Options { get { return (BinanceTRRestApiOptions)this.ClientOptions; } }
    internal TimeSyncState TimeSyncState { get; } = new("BinanceTR Rest API");
    internal List<BinanceTRSymbol> Symbols { get; private set; } = [];
    internal Dictionary<string, BinanceTRSymbol> NormalizedSymbols { get; private set; } = [];
    internal ILogger Logger { get => _logger; }

    /// <summary>
    /// Binance TR Rest API Client
    /// </summary>
    public BinanceTRRestApiClient() : this(new BinanceTRRestApiOptions(), null) { }

    /// <summary>
    /// Binance TR Rest API Client
    /// </summary>
    /// <param name="options">Client Options</param>
    /// <param name="logger">Logger</param>
    public BinanceTRRestApiClient(BinanceTRRestApiOptions options, ILogger logger = null) : base(logger, options)
    {
        ManualParseError = true;
        RequestBodyFormat = RestRequestBodyFormat.FormData;
        ArraySerialization = ArraySerialization.MultipleValues;
    }

    #region Overrides
    /// <inheritdoc/>
    protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials) => new BinanceTRAuthenticationProvider(credentials);
    /// <inheritdoc/>
    protected override Task<RestCallResult<DateTime>> GetServerTimestampAsync() => this.GetTimeAsync();
    /// <inheritdoc/>
    protected override TimeSyncInfo GetTimeSyncInfo() => new(Logger, Options.AutoTimestamp, Options.AutoTimestampInterval, TimeSyncState);
    /// <inheritdoc/>
    protected override TimeSpan GetTimeOffset() => TimeSyncState.TimeOffset;
    /// <inheritdoc/>
    protected override Error ParseErrorResponse(JToken error)
    {
        if (!error.HasValues)
            return new ServerError(error.ToString());

        if (error["code"] == null || error["msg"] == null)
            return new ServerError(error.ToString());

        return new ServerError((int)error["code"]!, (string)error["msg"]!, error.ToString());
    }
    /// <inheritdoc/>
    protected override Task<ServerError> TryParseErrorAsync(JToken error)
    {
        if (!error.HasValues)
            return Task.FromResult<ServerError>(null);

        if (error["code"] == null || (int)error["code"] == 0)
            return Task.FromResult<ServerError>(null);

        if (error["code"] != null && (int)error["code"] != 0 && error["msg"] != null)
            return Task.FromResult(new ServerError((int)error["code"]!, (string)error["msg"]!, error.ToString()));

        return Task.FromResult<ServerError>(null);
    }
    #endregion

    #region Protected Methods
    /// <summary>
    /// Request
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="uri"></param>
    /// <param name="method"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="signed"></param>
    /// <param name="queryParameters"></param>
    /// <param name="bodyParameters"></param>
    /// <param name="headerParameters"></param>
    /// <param name="arraySerialization"></param>
    /// <param name="deserializer"></param>
    /// <param name="ignoreRatelimit"></param>
    /// <param name="requestWeight"></param>
    /// <returns></returns>
    protected async Task<RestCallResult<T>> RequestAsync<T>(Uri uri, HttpMethod method, CancellationToken cancellationToken, bool signed = false, ParameterCollection queryParameters = null, ParameterCollection bodyParameters = null, Dictionary<string, string> headerParameters = null, ArraySerialization? arraySerialization = null, JsonSerializer deserializer = null, bool ignoreRatelimit = false, int requestWeight = 1)
    {
        // Get Original Cultures
        var currentCulture = Thread.CurrentThread.CurrentCulture;
        var currentUICulture = Thread.CurrentThread.CurrentUICulture;

        // Set Cultures
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

        // Do Request
        var result = await SendRequestAsync<T>(uri, method, cancellationToken, signed, queryParameters, bodyParameters, headerParameters, arraySerialization, deserializer, ignoreRatelimit, requestWeight).ConfigureAwait(false);

        // Set Orifinal Cultures
        Thread.CurrentThread.CurrentCulture = currentCulture;
        Thread.CurrentThread.CurrentUICulture = currentUICulture;

        // Return
        if (!result.Success || result.Data == null) return new RestCallResult<T>(result.Request, result.Response, result.Raw, result.Error);
        return new RestCallResult<T>(result.Request, result.Response, result.Data, result.Raw, result.Error);
    }

    /// <summary>
    /// Payload Request
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="uri"></param>
    /// <param name="method"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="signed"></param>
    /// <param name="queryParameters"></param>
    /// <param name="bodyParameters"></param>
    /// <param name="headerParameters"></param>
    /// <param name="arraySerialization"></param>
    /// <param name="deserializer"></param>
    /// <param name="ignoreRatelimit"></param>
    /// <param name="requestWeight"></param>
    /// <returns></returns>
    protected async Task<RestCallResult<T>> PayloadRequestAsync<T>(Uri uri, HttpMethod method, CancellationToken cancellationToken, bool signed = false, ParameterCollection queryParameters = null, ParameterCollection bodyParameters = null, Dictionary<string, string> headerParameters = null, ArraySerialization? arraySerialization = null, JsonSerializer deserializer = null, bool ignoreRatelimit = false, int requestWeight = 1)
    {
        // Get Original Cultures
        var currentCulture = Thread.CurrentThread.CurrentCulture;
        var currentUICulture = Thread.CurrentThread.CurrentUICulture;

        // Set Cultures
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

        // Do Request
        var result = await SendRequestAsync<BinanceTRRestApiResponse<T>>(uri, method, cancellationToken, signed, queryParameters, bodyParameters, headerParameters, arraySerialization, deserializer, ignoreRatelimit, requestWeight).ConfigureAwait(false);

        // Set Orifinal Cultures
        Thread.CurrentThread.CurrentCulture = currentCulture;
        Thread.CurrentThread.CurrentUICulture = currentUICulture;

        // Return
        if (!result.Success || result.Data == null) return new RestCallResult<T>(result.Request, result.Response, result.Raw, result.Error);
        if (result.Data.Code != 0) return new RestCallResult<T>(result.Request, result.Response, result.Raw, new ServerError(result.Data.Code, result.Data.Message));
        return new RestCallResult<T>(result.Request, result.Response, result.Data.Payload, result.Raw, result.Error);
    }

    /// <summary>
    /// Build Uri
    /// </summary>
    /// <param name="datacenter">Binance Data Center</param>
    /// <param name="parameters">URL Path Items</param>
    /// <returns></returns>
    protected Uri BuildUri(BinanceDataCenter datacenter, params string[] parameters)
    {
        var baseAddress = "";
        if (datacenter == BinanceDataCenter.ME) baseAddress = string.IsNullOrEmpty(MeRestApiAddress) ? BinanceTRAddress.Default.MeRestApiAddress : MeRestApiAddress;
        if (datacenter == BinanceDataCenter.TR) baseAddress = string.IsNullOrEmpty(TrRestApiAddress) ? BinanceTRAddress.Default.TrRestApiAddress : TrRestApiAddress;
        if (datacenter == BinanceDataCenter.App) baseAddress = string.IsNullOrEmpty(AppRestApiAddress) ? BinanceTRAddress.Default.AppRestApiAddress : AppRestApiAddress;

        return new Uri($"{baseAddress.TrimEnd('/')}/{string.Join("/", parameters).TrimStart('/')}");
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Set API Credentials
    /// </summary>
    /// <param name="key">API Key</param>
    /// <param name="secret">API Secret</param>
    public void SetApiCredentials(string key, string secret) => base.SetApiCredentials(new ApiCredentials(key, secret));
    #endregion

    #region Rest API Methods
    /// <summary>
    /// Get Server Time
    /// </summary>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    public async Task<RestCallResult<DateTime>> GetTimeAsync(CancellationToken ct = default)
    {
        var result = await RequestAsync<BinanceTRTime>(BuildUri(BinanceDataCenter.TR, "open/v1/common/time"), HttpMethod.Get, ct).ConfigureAwait(false);
        if (!result) return result.AsError<DateTime>(result.Error);
        return result.As(result.Data.Time);
    }

    /// <summary>
    /// Get Exchange Information
    /// </summary>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    public async Task<RestCallResult<List<BinanceTRSymbol>>> GetSymbolsAsync(CancellationToken ct = default)
    {
        var result = await PayloadRequestAsync<BinanceTRRestApiListResponse<BinanceTRSymbol>>(BuildUri(BinanceDataCenter.TR, "open/v1/common/symbols"), HttpMethod.Get, ct).ConfigureAwait(false);
        if (!result) return result.AsError<List<BinanceTRSymbol>>(result.Error);

        Symbols = result.Data.List.ToList();
        NormalizedSymbols = Symbols.ToDictionary(s => GetNormalizedSymbol(s.Symbol), s => s);

        return result.As(result.Data.List);
    }

    internal async Task<BinanceTRSymbol> GetSymbolAsync(string symbol, CancellationToken ct = default)
    {
        if (Symbols.Count == 0) await GetSymbolsAsync(ct).ConfigureAwait(false);
        if (!NormalizedSymbols.TryGetValue(GetNormalizedSymbol(symbol), out var item))
            return null;

        return item;
    }

    internal string GetNormalizedSymbol(string symbol)
    {
        return symbol.Replace("-", "").Replace("_", "");
    }

    internal async Task<string> GetExchangeInfoSymbol(string symbol, CancellationToken ct = default)
    {
        var sym = await GetSymbolAsync(symbol, ct);
        return sym != null ? sym.Symbol : symbol;
    }

    /// <summary>
    /// Get Order Book
    /// </summary>
    /// <param name="symbol">Symbol</param>
    /// <param name="limit">Limit</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    public async Task<RestCallResult<BinanceTROrderBook>> GetOrderBookAsync(string symbol, int limit = 100, CancellationToken ct = default)
    {
        // Normalized Symbol
        symbol = GetNormalizedSymbol(symbol);

        // Validations
        limit.ValidateIntValues(nameof(limit), 5, 10, 20, 50, 100, 500, 1000, 5000);

        // Parameters
        var parameters = new ParameterCollection();
        parameters.AddParameter("symbol", symbol);
        parameters.AddOptional("limit", limit);

        // Get Uri
        var sym = await GetSymbolAsync(symbol, ct);
        var uri = sym != null && sym.Type == SymbolType.Main
            ? BuildUri(BinanceDataCenter.ME, "api/v3/depth")
            : BuildUri(BinanceDataCenter.App, "api/v1/depth");

        // Do Request
        var result = await RequestAsync<BinanceTROrderBook>(uri, HttpMethod.Get, ct, false, parameters).ConfigureAwait(false);
        if (!result) return result.AsError<BinanceTROrderBook>(result.Error);

        // Set Symbol
        result.Data.Symbol = symbol;

        // Return
        return result.As(result.Data);
    }

    /// <summary>
    /// Get Recent Trades
    /// </summary>
    /// <param name="symbol">Symbol</param>
    /// <param name="fromId">From ID</param>
    /// <param name="limit">Limit</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    public async Task<RestCallResult<List<BinanceTRTrade>>> GetTradesAsync(string symbol, long? fromId = null, int limit = 500, CancellationToken ct = default)
    {
        // Normalized Symbol
        symbol = GetNormalizedSymbol(symbol);

        // Validations
        limit.ValidateIntBetween(nameof(limit), 1, 1000);

        // Parameters
        var parameters = new ParameterCollection();
        parameters.AddParameter("symbol", symbol);
        parameters.AddOptional("fromId", fromId);
        parameters.AddOptional("limit", limit);

        // Get Uri
        var sym = await GetSymbolAsync(symbol, ct);
        var uri = sym != null && sym.Type == SymbolType.Main
            ? BuildUri(BinanceDataCenter.ME, "api/v3/trades")
            : BuildUri(BinanceDataCenter.TR, "open/v1/market/trades");

        // Do Request
        var result = await RequestAsync<List<BinanceTRTrade>>(uri, HttpMethod.Get, ct, false, parameters).ConfigureAwait(false);

        // Set Symbol
        if (result) result.Data.ForEach(t => t.Symbol = symbol);

        // Return
        return result;
    }

    /// <summary>
    /// Get Aggregated Trades
    /// </summary>
    /// <param name="symbol">Symbol</param>
    /// <param name="fromId">From ID</param>
    /// <param name="startTime">Start Time</param>
    /// <param name="endTime">End Time</param>
    /// <param name="limit">Limit</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    public async Task<RestCallResult<List<BinanceTRAggregatedTrade>>> GetAggregatedTradesAsync(string symbol, long? fromId = null, DateTime? startTime = null, DateTime? endTime = null, int limit = 500, CancellationToken ct = default)
    {
        // Normalized Symbol
        symbol = GetNormalizedSymbol(symbol);

        // Validations
        limit.ValidateIntBetween(nameof(limit), 1, 1000);

        // Parameters
        var parameters = new ParameterCollection();
        parameters.AddParameter("symbol", symbol);
        parameters.AddOptional("fromId", fromId);
        parameters.AddOptional("startTime", startTime?.ConvertToMilliseconds());
        parameters.AddOptional("endTime", endTime?.ConvertToMilliseconds());
        parameters.AddOptional("limit", limit);

        // Get Uri
        var sym = await GetSymbolAsync(symbol, ct);
        var uri = sym != null && sym.Type == SymbolType.Main
            ? BuildUri(BinanceDataCenter.ME, "api/v3/aggTrades")
            : BuildUri(BinanceDataCenter.App, "api/v1/aggTrades");

        // Do Request
        var result = await RequestAsync<List<BinanceTRAggregatedTrade>>(uri, HttpMethod.Get, ct, false, parameters).ConfigureAwait(false);

        // Set Symbol
        if (result) result.Data.ForEach(t => t.Symbol = symbol);

        // Return
        return result;
    }

    /// <summary>
    /// Get Klines
    /// </summary>
    /// <param name="symbol">Symbol</param>
    /// <param name="interval">Interval</param>
    /// <param name="startTime">Start Time</param>
    /// <param name="endTime">End Time</param>
    /// <param name="limit">Limit</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    public async Task<RestCallResult<List<BinanceTRKline>>> GetKlinesAsync(string symbol, KlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int limit = 500, CancellationToken ct = default)
    {
        // Normalized Symbol
        symbol = GetNormalizedSymbol(symbol);

        // Validations
        limit.ValidateIntBetween(nameof(limit), 1, 1000);

        // Parameters
        var parameters = new ParameterCollection();
        parameters.AddParameter("symbol", symbol);
        parameters.AddEnum("interval", interval);
        parameters.AddOptional("startTime", startTime?.ConvertToMilliseconds());
        parameters.AddOptional("endTime", endTime?.ConvertToMilliseconds());
        parameters.AddOptional("limit", limit);

        // Get Uri
        var sym = await GetSymbolAsync(symbol, ct);
        var uri = sym != null && sym.Type == SymbolType.Main
            ? BuildUri(BinanceDataCenter.ME, "api/v1/klines")
            : BuildUri(BinanceDataCenter.App, "api/v1/klines");

        // Do Request
        var result = await RequestAsync<List<BinanceTRKline>>(uri, HttpMethod.Get, ct, false, parameters).ConfigureAwait(false);

        // Set Symbol
        if (result) result.Data.ForEach(t => t.Symbol = symbol);

        // Return
        return result;
    }

    /// <summary>
    /// Place Order
    /// </summary>
    /// <param name="symbol">Symbol</param>
    /// <param name="side">Order Side</param>
    /// <param name="type">Order Type</param>
    /// <param name="quantity">Base Quantity</param>
    /// <param name="quoteQuantity">Quote Quantity</param>
    /// <param name="icebergQuantity">Iceberg Quantity</param>
    /// <param name="price">Price</param>
    /// <param name="stopPrice">Stop Price</param>
    /// <param name="clientOrderId">Client Order Id</param>
    /// <param name="timeInForce">Time In Force</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    public async Task<RestCallResult<BinanceTROrder>> PlaceOrderAsync(
        string symbol,
        OrderSide side,
        OrderType type,
        decimal? quantity = null,
        decimal? quoteQuantity = null,
        decimal? icebergQuantity = null,
        decimal? price = null,
        decimal? stopPrice = null,
        string clientOrderId = null,
        TimeInForce? timeInForce = null,
        CancellationToken ct = default)
    {
        // ExchangeInfo Symbol
        symbol = await GetExchangeInfoSymbol(symbol, ct);

        // Parameters
        var parameters = new ParameterCollection();
        parameters.AddParameter("symbol", symbol);
        parameters.AddEnum("side", side);
        parameters.AddEnum("type", type);
        parameters.AddOptional("quantity", quantity);
        parameters.AddOptional("quoteOrderQty", quoteQuantity);
        parameters.AddOptional("icebergQty", icebergQuantity);
        parameters.AddOptional("price", price);
        parameters.AddOptional("stopPrice", stopPrice);
        parameters.AddOptional("clientId", clientOrderId);
        parameters.AddOptionalEnum("timeInForce", timeInForce);

        // Do Request
        return await PayloadRequestAsync<BinanceTROrder>(BuildUri(BinanceDataCenter.TR, "open/v1/orders"), HttpMethod.Post, ct, true, null, parameters);
    }

    /// <summary>
    /// Get Order
    /// </summary>
    /// <param name="orderId">Order Id</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    public Task<RestCallResult<BinanceTROrder>> GetOrderAsync(long orderId, CancellationToken ct = default)
    {
        // Parameters
        var parameters = new ParameterCollection();
        parameters.AddParameter("orderId", orderId);

        // Do Request
        return PayloadRequestAsync<BinanceTROrder>(BuildUri(BinanceDataCenter.TR, "open/v1/orders/detail"), HttpMethod.Get, ct, true, parameters);
    }

    /// <summary>
    /// Cancel Order
    /// </summary>
    /// <param name="orderId">Order Id</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    public Task<RestCallResult<BinanceTROrder>> CancelOrderAsync(long orderId, CancellationToken ct = default)
    {
        // Parameters
        var parameters = new ParameterCollection();
        parameters.AddParameter("orderId", orderId);

        // Do Request
        return PayloadRequestAsync<BinanceTROrder>(BuildUri(BinanceDataCenter.TR, "open/v1/orders/cancel"), HttpMethod.Post, ct, true, parameters);
    }

    /// <summary>
    /// Get Orders
    /// </summary>
    /// <param name="symbol">Symbol</param>
    /// <param name="side">Order Side</param>
    /// <param name="type">Order Query Type</param>
    /// <param name="direction">Direction</param>
    /// <param name="fromId">From ID</param>
    /// <param name="startTime">Start Time</param>
    /// <param name="endTime">End Time</param>
    /// <param name="limit">Limit</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    public async Task<RestCallResult<List<BinanceTROrder>>> GetOrdersAsync(
        string symbol = null,
        OrderSide? side = null,
        OrderQuery? type = null,
        QueryDirection? direction = null,
        long? fromId = null,
        DateTime? startTime = null,
        DateTime? endTime = null,
        int limit = 500,
        CancellationToken ct = default)
    {
        // ExchangeInfo Symbol
        if (!string.IsNullOrEmpty(symbol))
            symbol = await GetExchangeInfoSymbol(symbol, ct);

        // Validations
        limit.ValidateIntBetween(nameof(limit), 1, 1000);

        // Parameters
        var parameters = new ParameterCollection();
        parameters.AddOptional("symbol", symbol);
        parameters.AddOptionalEnum("side", side);
        parameters.AddOptionalEnum("type", type);
        parameters.AddOptionalEnum("direct", direction);
        parameters.AddOptional("startTime", startTime?.ConvertToMilliseconds());
        parameters.AddOptional("endTime", endTime?.ConvertToMilliseconds());
        parameters.AddOptional("fromId", fromId);
        parameters.AddOptional("limit", limit);

        // Do Request
        var result = await PayloadRequestAsync<BinanceTRRestApiListResponse<BinanceTROrder>>(BuildUri(BinanceDataCenter.TR, "open/v1/orders"), HttpMethod.Get, ct, true, parameters).ConfigureAwait(false);
        if (!result) return result.AsError<List<BinanceTROrder>>(result.Error);

        // Return
        return result.As(result.Data.List);
    }

    /// <summary>
    /// Get Open Orders
    /// </summary>
    /// <param name="symbol">Symbol</param>
    /// <param name="side">Order Side</param>
    /// <param name="direction">Direction</param>
    /// <param name="fromId">From ID</param>
    /// <param name="startTime">Start Time</param>
    /// <param name="endTime">End Time</param>
    /// <param name="limit">Limit</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    public async Task<RestCallResult<List<BinanceTROrder>>> GetOpenOrdersAsync(
        string symbol = null,
        OrderSide? side = null,
        QueryDirection? direction = null,
        long? fromId = null,
        DateTime? startTime = null,
        DateTime? endTime = null,
        int limit = 500,
        CancellationToken ct = default)
    {
        // ExchangeInfo Symbol
        if (!string.IsNullOrEmpty(symbol))
            symbol = await GetExchangeInfoSymbol(symbol, ct);

        // Validations
        limit.ValidateIntBetween(nameof(limit), 1, 1000);

        // Parameters
        var parameters = new ParameterCollection();
        parameters.AddOptional("symbol", symbol);
        parameters.AddOptionalEnum("side", side);
        parameters.AddOptionalEnum("type", OrderQuery.Open);
        parameters.AddOptionalEnum("direct", direction);
        parameters.AddOptional("startTime", startTime?.ConvertToMilliseconds());
        parameters.AddOptional("endTime", endTime?.ConvertToMilliseconds());
        parameters.AddOptional("fromId", fromId);
        parameters.AddOptional("limit", limit);

        // Do Request
        var result = await PayloadRequestAsync<BinanceTRRestApiListResponse<BinanceTROrder>>(BuildUri(BinanceDataCenter.TR, "open/v1/orders"), HttpMethod.Get, ct, true, parameters).ConfigureAwait(false);
        if (!result) return result.AsError<List<BinanceTROrder>>(result.Error);

        // Return
        return result.As(result.Data.List);
    }

    /// <summary>
    /// Place OCO Order
    /// </summary>
    /// <param name="symbol">Symbol</param>
    /// <param name="side">Order Side</param>
    /// <param name="quantity">Quantity</param>
    /// <param name="price">Price</param>
    /// <param name="stopPrice">Stop Price</param>
    /// <param name="stopLimitPrice">Stop Limit Price</param>
    /// <param name="stopClientOrderId">Stop Client Order Id</param>
    /// <param name="listClientOrderId">List Client Order Id</param>
    /// <param name="limitClientOrderId">Limit Client Order Id</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    public async Task<RestCallResult<BinanceTROrderId>> PlaceOcoOrderAsync(
        string symbol,
        OrderSide side,
        decimal quantity,
        decimal price,
        decimal stopPrice,
        decimal stopLimitPrice,
        string stopClientOrderId = null,
        string listClientOrderId = null,
        string limitClientOrderId = null,
        CancellationToken ct = default)
    {
        // ExchangeInfo Symbol
        symbol = await GetExchangeInfoSymbol(symbol, ct);

        // Parameters
        var parameters = new ParameterCollection();
        parameters.AddParameter("symbol", symbol);
        parameters.AddEnum("side", side);
        parameters.AddParameter("quantity", quantity);
        parameters.AddParameter("price", price);
        parameters.AddParameter("stopPrice", stopPrice);
        parameters.AddParameter("stopLimitPrice", stopLimitPrice);
        parameters.AddOptional("stopClientId", stopClientOrderId);
        parameters.AddOptional("listClientId", listClientOrderId);
        parameters.AddOptional("limitClientId", limitClientOrderId);

        // Do Request
        return await PayloadRequestAsync<BinanceTROrderId>(BuildUri(BinanceDataCenter.TR, "open/v1/orders/oco"), HttpMethod.Post, ct, true, null, parameters);
    }

    /// <summary>
    /// Get Account Information
    /// </summary>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    public Task<RestCallResult<BinanceTRAccount>> GetAccountAsync(CancellationToken ct = default)
    {
        // Do Request
        return PayloadRequestAsync<BinanceTRAccount>(BuildUri(BinanceDataCenter.TR, "open/v1/account/spot"), HttpMethod.Get, ct, true);
    }

    /// <summary>
    /// Get Account Balances
    /// </summary>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    public async Task<RestCallResult<List<BinanceTRAccountBalance>>> GetBalancesAsync(CancellationToken ct = default)
    {
        // Do Request
        var result = await PayloadRequestAsync<BinanceTRAccount>(BuildUri(BinanceDataCenter.TR, "open/v1/account/spot"), HttpMethod.Get, ct, true).ConfigureAwait(false);
        if (!result) return result.AsError<List<BinanceTRAccountBalance>>(result.Error);

        // Return
        return result.As(result.Data.Balances);
    }

    /// <summary>
    /// Get Account Balance
    /// </summary>
    /// <param name="asset">Asset</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    public Task<RestCallResult<BinanceTRAccountBalance>> GetBalanceAsync(string asset, CancellationToken ct = default)
    {
        // Parameters
        var parameters = new ParameterCollection();
        parameters.AddParameter("asset", asset);

        // Do Request
        return PayloadRequestAsync<BinanceTRAccountBalance>(BuildUri(BinanceDataCenter.TR, "open/v1/account/spot/asset"), HttpMethod.Get, ct, true);
    }

    /// <summary>
    /// Get Account Trades
    /// </summary>
    /// <param name="symbol">Symbol</param>
    /// <param name="orderId">Order ID</param>
    /// <param name="direction">Direction</param>
    /// <param name="startTime">Start Time</param>
    /// <param name="endTime">End Time</param>
    /// <param name="fromId">From ID</param>
    /// <param name="limit">Limit</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    public async Task<RestCallResult<List<BinanceTRAccountTrade>>> GetAccountTradesAsync(
        string symbol,
        long? orderId = null,
        QueryDirection? direction = null,
        DateTime? startTime = null,
        DateTime? endTime = null,
        long? fromId = null,
        int limit = 500,
        CancellationToken ct = default)
    {
        // ExchangeInfo Symbol
        symbol = await GetExchangeInfoSymbol(symbol, ct);

        // Validations
        limit.ValidateIntBetween(nameof(limit), 1, 1000);

        // Parameters
        var parameters = new ParameterCollection();
        parameters.AddParameter("symbol", symbol);
        parameters.AddOptional("orderId", orderId);
        parameters.AddOptionalEnum("direct", direction);
        parameters.AddOptional("startTime", startTime?.ConvertToMilliseconds());
        parameters.AddOptional("endTime", endTime?.ConvertToMilliseconds());
        parameters.AddOptional("fromId", fromId);
        parameters.AddOptional("limit", limit);

        // Do Request
        var result = await PayloadRequestAsync<BinanceTRRestApiListResponse<BinanceTRAccountTrade>>(BuildUri(BinanceDataCenter.TR, "open/v1/orders/trades"), HttpMethod.Get, ct, true, parameters).ConfigureAwait(false);
        if (!result) return result.AsError<List<BinanceTRAccountTrade>>(result.Error);

        // Return
        return result.As(result.Data.List);
    }

    /// <summary>
    /// Withdraw
    /// </summary>
    /// <param name="asset">Asset</param>
    /// <param name="amount">Quantity</param>
    /// <param name="address">Address</param>
    /// <param name="addressTag">Tag/Memo</param>
    /// <param name="network">Network</param>
    /// <param name="clientWithdrawId">Client Withdraw Id</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    public Task<RestCallResult<BinanceTRWithdrawalId>> WithdrawAsync(
        string asset,
        decimal amount,
        string address,
        string addressTag = null,
        string network = null,
        string clientWithdrawId = null,
        CancellationToken ct = default)
    {
        // Parameters
        var parameters = new ParameterCollection();
        parameters.AddParameter("asset", asset);
        parameters.AddParameter("amount", amount);
        parameters.AddParameter("address", address);
        parameters.AddOptional("addressTag", addressTag);
        parameters.AddOptional("network", network);
        parameters.AddOptional("clientId", clientWithdrawId);

        // Do Request
        return PayloadRequestAsync<BinanceTRWithdrawalId>(BuildUri(BinanceDataCenter.TR, "open/v1/withdraws"), HttpMethod.Post, ct, true, parameters);
    }

    /// <summary>
    /// Get Withdrawals
    /// </summary>
    /// <param name="asset">Asset</param>
    /// <param name="status">Status</param>
    /// <param name="fromId">From Id</param>
    /// <param name="startTime">Start Time</param>
    /// <param name="endTime">End Time</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    public async Task<RestCallResult<List<BinanceTRWithdrawal>>> GetWithdrawalsAsync(
        string asset = null,
        WithdrawalStatus? status = null,
        long? fromId = null,
        DateTime? startTime = null,
        DateTime? endTime = null,
        CancellationToken ct = default)
    {
        // Parameters
        var parameters = new ParameterCollection();
        parameters.AddOptional("asset", asset);
        parameters.AddOptionalEnum("status", status);
        parameters.AddOptional("fromId", fromId);
        parameters.AddOptional("startTime", startTime?.ConvertToMilliseconds());
        parameters.AddOptional("endTime", endTime?.ConvertToMilliseconds());

        // Do Request
        var result = await PayloadRequestAsync<BinanceTRRestApiListResponse<BinanceTRWithdrawal>>(BuildUri(BinanceDataCenter.TR, "open/v1/withdraws"), HttpMethod.Get, ct, true, parameters).ConfigureAwait(false);
        if (!result) return result.AsError<List<BinanceTRWithdrawal>>(result.Error);

        // Return
        return result.As(result.Data.List);
    }

    /// <summary>
    /// Get Deposits
    /// </summary>
    /// <param name="asset">Asset</param>
    /// <param name="status">Status</param>
    /// <param name="fromId">From Id</param>
    /// <param name="startTime">Start Time</param>
    /// <param name="endTime">End Time</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    public async Task<RestCallResult<List<BinanceTRDeposit>>> GetDepositsAsync(
        string asset = null,
        DepositStatus? status = null,
        long? fromId = null,
        DateTime? startTime = null,
        DateTime? endTime = null,
        CancellationToken ct = default)
    {
        // Parameters
        var parameters = new ParameterCollection();
        parameters.AddOptional("asset", asset);
        parameters.AddOptionalEnum("status", status);
        parameters.AddOptional("fromId", fromId);
        parameters.AddOptional("startTime", startTime?.ConvertToMilliseconds());
        parameters.AddOptional("endTime", endTime?.ConvertToMilliseconds());

        // Do Request
        var result = await PayloadRequestAsync<BinanceTRRestApiListResponse<BinanceTRDeposit>>(BuildUri(BinanceDataCenter.TR, "open/v1/deposits"), HttpMethod.Get, ct, true, parameters).ConfigureAwait(false);
        if (!result) return result.AsError<List<BinanceTRDeposit>>(result.Error);

        // Return
        return result.As(result.Data.List);
    }

    /// <summary>
    /// Get Deposit Address
    /// </summary>
    /// <param name="asset">Asset</param>
    /// <param name="network">Network</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    public Task<RestCallResult<BinanceTRDepositAddress>> GetDepositAddressAsync(
        string asset = null,
        string network = null,
        CancellationToken ct = default)
    {
        // Parameters
        var parameters = new ParameterCollection();
        parameters.AddParameter("asset", asset);
        parameters.AddParameter("network", network);

        // Do Request
        return PayloadRequestAsync<BinanceTRDepositAddress>(BuildUri(BinanceDataCenter.TR, "open/v1/deposits/address"), HttpMethod.Get, ct, true, parameters);
    }

    /// <summary>
    /// Create Listen Key
    /// </summary>
    /// <param name="type">Symbol Type</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    public Task<RestCallResult<string>> CreateListenKeyAsync(SymbolType type = SymbolType.Main, CancellationToken ct = default)
    {
        // Get Uri
        var uri = type == SymbolType.Main
            ? BuildUri(BinanceDataCenter.TR, "open/v1/user-data-stream")
            : BuildUri(BinanceDataCenter.TR, "open/v1/private-n/user-data-stream");

        // Do Request
        return PayloadRequestAsync<string>(uri, HttpMethod.Post, ct, true);
    }

    /// <summary>
    /// Extend Listen Key
    /// </summary>
    /// <param name="listenKey">Listen Key</param>
    /// <param name="type">Symbol Type</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    public async Task<RestCallResult<bool>> ExtendListenKeyAsync(string listenKey, SymbolType type = SymbolType.Main, CancellationToken ct = default)
    {
        // Parameters
        var parameters = new ParameterCollection();
        parameters.AddParameter("listenKey", listenKey);

        // Get Uri
        var uri = type == SymbolType.Main
            ? BuildUri(BinanceDataCenter.TR, "open/v1/user-data-stream")
            : BuildUri(BinanceDataCenter.TR, "open/v1/private-n/user-data-stream");

        // Do Request
        var result = await RequestAsync<object>(uri, HttpMethod.Put, ct, true, null, parameters).ConfigureAwait(false);
        if (!result) return result.AsError<bool>(result.Error);

        // Return
        return result.As(true);
    }

    /// <summary>
    /// Close Listen Key
    /// </summary>
    /// <param name="listenKey">Listen Key</param>
    /// <param name="type">Symbol Type</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    public async Task<RestCallResult<bool>> CloseListenKeyAsync(string listenKey, SymbolType type = SymbolType.Main, CancellationToken ct = default)
    {
        // Parameters
        var parameters = new ParameterCollection();
        parameters.AddParameter("listenKey", listenKey);

        // Get Uri
        var uri = type == SymbolType.Main
            ? BuildUri(BinanceDataCenter.TR, "open/v1/user-data-stream")
            : BuildUri(BinanceDataCenter.TR, "open/v1/private-n/user-data-stream");

        // Do Request
        var result = await RequestAsync<object>(uri, HttpMethod.Delete, ct, true, null, parameters).ConfigureAwait(false);
        if (!result) return result.AsError<bool>(result.Error);

        // Return
        return result.As(true);
    }
    #endregion
}
