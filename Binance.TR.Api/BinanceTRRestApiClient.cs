namespace Binance.TR.Api;

public class BinanceTRRestApiClient : RestApiClient
{
    internal BinanceTRRestApiOptions Options { get { return (BinanceTRRestApiOptions)this.ClientOptions; } }
    internal TimeSyncState TimeSyncState { get; } = new("BinanceTR Rest API");
    internal List<BinanceTRSymbol> Symbols { get; private set; } = [];
    internal ILogger Logger { get => _logger; }

    public BinanceTRRestApiClient() : this(new BinanceTRRestApiOptions(), null) { }
    public BinanceTRRestApiClient(BinanceTRRestApiOptions options, ILogger logger = null) : base(logger, options)
    {
        ManualParseError = true;
        RequestBodyFormat = RestRequestBodyFormat.FormData;
        ArraySerialization = ArraySerialization.MultipleValues;
    }

    #region Overrides
    protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials) => new BinanceTRAuthenticationProvider(credentials);
    protected override Task<RestCallResult<DateTime>> GetServerTimestampAsync() => this.GetTimeAsync();
    protected override TimeSyncInfo GetTimeSyncInfo() => new(Logger, Options.AutoTimestamp, Options.AutoTimestampInterval, TimeSyncState);
    protected override TimeSpan GetTimeOffset() => TimeSyncState.TimeOffset;
    protected override Error ParseErrorResponse(JToken error)
    {
        if (!error.HasValues)
            return new ServerError(error.ToString());

        if (error["code"] == null || error["msg"] == null)
            return new ServerError(error.ToString());

        return new ServerError((int)error["code"]!, (string)error["msg"]!, error.ToString());
    }
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
    protected Uri BuildUri(BinanceDataCenter datacenter, params string[] parameters)
    {
        var baseAddress = "";
        if (datacenter == BinanceDataCenter.ME) baseAddress = BinanceTRAddress.Default.MeRestApiAddress;
        if (datacenter == BinanceDataCenter.TR) baseAddress = BinanceTRAddress.Default.TrRestApiAddress;
        if (datacenter == BinanceDataCenter.App) baseAddress = BinanceTRAddress.Default.AppRestApiAddress;

        return new Uri($"{baseAddress.TrimEnd('/')}/{string.Join("/", parameters).TrimStart('/')}");
    }
    #endregion

    #region Public Methods
    public void SetApiCredentials(string key, string secret) => base.SetApiCredentials(new ApiCredentials(key, secret));
    #endregion

    #region Rest API Methods
    public async Task<RestCallResult<DateTime>> GetTimeAsync(CancellationToken ct = default)
    {
        var result = await RequestAsync<BinanceTRTime>(BuildUri(BinanceDataCenter.TR, "open/v1/common/time"), HttpMethod.Get, ct).ConfigureAwait(false);
        if (!result) return result.AsError<DateTime>(result.Error);
        return result.As(result.Data.Time);
    }

    public async Task<RestCallResult<List<BinanceTRSymbol>>> GetSymbolsAsync(CancellationToken ct = default)
    {
        var result = await PayloadRequestAsync<BinanceTRRestApiListResponse<BinanceTRSymbol>>(BuildUri(BinanceDataCenter.TR, "open/v1/common/symbols"), HttpMethod.Get, ct).ConfigureAwait(false);
        if (!result) return result.AsError<List<BinanceTRSymbol>>(result.Error);

        Symbols = result.Data.List.ToList();
        Symbols.ForEach(s => s.Symbol = s.Symbol.Replace("-", "").Replace("_", ""));

        return result.As(result.Data.List);
    }

    internal async Task<BinanceTRSymbol> GetSymbolAsync(string symbol, CancellationToken ct = default)
    {
        symbol = symbol.Replace("-", "").Replace("_", "");
        if (Symbols.Count == 0) await GetSymbolsAsync(ct).ConfigureAwait(false);
        return Symbols.SingleOrDefault(s => s.Symbol == symbol);
    }

    public async Task<RestCallResult<BinanceTROrderBook>> GetOrderBookAsync(string symbol, int limit = 100, CancellationToken ct = default)
    {
        // Symbol
        symbol = symbol.Replace("-", "").Replace("_", "");

        // Validations
        limit.ValidateIntValues(nameof(limit), 5, 10, 20, 50, 100, 500, 1000, 5000);

        // Parameters
        var parameters = new ParameterCollection();
        parameters.Add("symbol", symbol);
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

    public async Task<RestCallResult<List<BinanceTRTrade>>> GetTradesAsync(string symbol, long? fromId = null, int limit = 500, CancellationToken ct = default)
    {
        // Symbol
        symbol = symbol.Replace("-", "").Replace("_", "");

        // Validations
        limit.ValidateIntBetween(nameof(limit), 1, 1000);

        // Parameters
        var parameters = new ParameterCollection();
        parameters.Add("symbol", symbol);
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

    public async Task<RestCallResult<List<BinanceTRAggregatedTrade>>> GetAggregatedTradesAsync(string symbol, long? fromId = null, DateTime? startTime = null, DateTime? endTime = null, int limit = 500, CancellationToken ct = default)
    {
        // Symbol
        symbol = symbol.Replace("-", "").Replace("_", "");

        // Validations
        limit.ValidateIntBetween(nameof(limit), 1, 1000);

        // Parameters
        var parameters = new ParameterCollection();
        parameters.Add("symbol", symbol);
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

    public async Task<RestCallResult<List<BinanceTRKline>>> GetKlinesAsync(string symbol, KlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int limit = 500, CancellationToken ct = default)
    {
        // Symbol
        symbol = symbol.Replace("-", "").Replace("_", "");

        // Validations
        limit.ValidateIntBetween(nameof(limit), 1, 1000);

        // Parameters
        var parameters = new ParameterCollection();
        parameters.Add("symbol", symbol);
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

    public Task<RestCallResult<BinanceTROrderId>> PlaceOrderAsync(
        string symbol,
        OrderSide side,
        OrderType type,
        decimal? quantity = null,
        decimal? quoteQuantity = null,
        decimal? icebergQuantity = null,
        decimal? price = null,
        decimal? stopPrice = null,
        string clientOrderId = null,
        CancellationToken ct = default)
    {
        // Parameters
        var parameters = new ParameterCollection();
        parameters.Add("symbol", symbol);
        parameters.AddEnum("side", side);
        parameters.AddEnum("type", type);
        parameters.AddOptional("quantity", quantity);
        parameters.AddOptional("quoteOrderQty", quoteQuantity);
        parameters.AddOptional("icebergQty", icebergQuantity);
        parameters.AddOptional("price", price);
        parameters.AddOptional("stopPrice", stopPrice);
        parameters.AddOptional("clientId", clientOrderId);

        // Do Request
        return PayloadRequestAsync<BinanceTROrderId>(BuildUri(BinanceDataCenter.TR, "open/v1/orders"), HttpMethod.Post, ct, true, null, parameters);
    }

    public Task<RestCallResult<BinanceTROrder>> GetOrderAsync(long orderId, CancellationToken ct = default)
    {
        // Parameters
        var parameters = new ParameterCollection();
        parameters.Add("orderId", orderId);

        // Do Request
        return PayloadRequestAsync<BinanceTROrder>(BuildUri(BinanceDataCenter.TR, "open/v1/orders/detail"), HttpMethod.Get, ct, true, parameters);
    }

    public Task<RestCallResult<BinanceTROrder>> CancelOrderAsync(long orderId, CancellationToken ct = default)
    {
        // Parameters
        var parameters = new ParameterCollection();
        parameters.Add("orderId", orderId);

        // Do Request
        return PayloadRequestAsync<BinanceTROrder>(BuildUri(BinanceDataCenter.TR, "open/v1/orders/cancel"), HttpMethod.Post, ct, true, parameters);
    }

    public async Task<RestCallResult<List<BinanceTROrder>>> GetOrdersAsync(
        string symbol,
        OrderSide? side = null,
        OrderQuery? type = null,
        QueryDirection? direction = null,
        long? fromId = null,
        DateTime? startTime = null,
        DateTime? endTime = null,
        int limit = 500,
        CancellationToken ct = default)
    {
        // Validations
        limit.ValidateIntBetween(nameof(limit), 1, 1000);

        // Parameters
        var parameters = new ParameterCollection();
        parameters.Add("symbol", symbol);
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

    public Task<RestCallResult<BinanceTROrderId>> PlaceOcoOrderAsync(
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
        // Parameters
        var parameters = new ParameterCollection();
        parameters.Add("symbol", symbol);
        parameters.AddEnum("side", side);
        parameters.Add("quantity", quantity);
        parameters.Add("price", price);
        parameters.Add("stopPrice", stopPrice);
        parameters.Add("stopLimitPrice", stopLimitPrice);
        parameters.AddOptional("stopClientId", stopClientOrderId);
        parameters.AddOptional("listClientId", listClientOrderId);
        parameters.AddOptional("limitClientId", limitClientOrderId);

        // Do Request
        return PayloadRequestAsync<BinanceTROrderId>(BuildUri(BinanceDataCenter.TR, "open/v1/orders/oco"), HttpMethod.Post, ct, true, null, parameters);
    }

    public Task<RestCallResult<BinanceTRAccount>> GetAccountAsync(CancellationToken ct = default)
    {
        // Do Request
        return PayloadRequestAsync<BinanceTRAccount>(BuildUri(BinanceDataCenter.TR, "open/v1/account/spot"), HttpMethod.Get, ct, true);
    }

    public async Task<RestCallResult<List<BinanceTRAccountBalance>>> GetBalancesAsync(CancellationToken ct = default)
    {
        // Do Request
        var result = await PayloadRequestAsync<BinanceTRAccount>(BuildUri(BinanceDataCenter.TR, "open/v1/account/spot"), HttpMethod.Get, ct, true).ConfigureAwait(false);
        if (!result) return result.AsError<List<BinanceTRAccountBalance>>(result.Error);

        // Return
        return result.As(result.Data.Balances);
    }

    public Task<RestCallResult<BinanceTRAccountBalance>> GetBalanceAsync(string asset, CancellationToken ct = default)
    {
        // Parameters
        var parameters = new ParameterCollection();
        parameters.Add("asset", asset);

        // Do Request
        return PayloadRequestAsync<BinanceTRAccountBalance>(BuildUri(BinanceDataCenter.TR, "open/v1/account/spot/asset"), HttpMethod.Get, ct, true);
    }

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
        // Validations
        limit.ValidateIntBetween(nameof(limit), 1, 1000);

        // Parameters
        var parameters = new ParameterCollection();
        parameters.Add("symbol", symbol);
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
        parameters.Add("asset", asset);
        parameters.Add("amount", amount);
        parameters.Add("address", address);
        parameters.AddOptional("addressTag", addressTag);
        parameters.AddOptional("network", network);
        parameters.AddOptional("clientId", clientWithdrawId);

        // Do Request
        return PayloadRequestAsync<BinanceTRWithdrawalId>(BuildUri(BinanceDataCenter.TR, "open/v1/withdraws"), HttpMethod.Post, ct, true, parameters);
    }

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

    public Task<RestCallResult<BinanceTRDepositAddress>> GetDepositAddressAsync(
        string asset = null,
        string network = null,
        CancellationToken ct = default)
    {
        // Parameters
        var parameters = new ParameterCollection();
        parameters.Add("asset", asset);
        parameters.Add("network", network);

        // Do Request
        return PayloadRequestAsync<BinanceTRDepositAddress>(BuildUri(BinanceDataCenter.TR, "open/v1/deposits/address"), HttpMethod.Get, ct, true, parameters);
    }

    public Task<RestCallResult<string>> CreateListenKeyAsync(SymbolType type = SymbolType.Main, CancellationToken ct = default)
    {
        // Get Uri
        var uri = type == SymbolType.Main
            ? BuildUri(BinanceDataCenter.TR, "open/v1/user-data-stream")
            : BuildUri(BinanceDataCenter.TR, "open/v1/private-n/user-data-stream");

        // Do Request
        return PayloadRequestAsync<string>(uri, HttpMethod.Post, ct, true);
    }

    public async Task<RestCallResult<bool>> ExtendListenKeyAsync(string listenKey, SymbolType type = SymbolType.Main, CancellationToken ct = default)
    {
        // Parameters
        var parameters = new ParameterCollection();
        parameters.Add("listenKey", listenKey);

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

    public async Task<RestCallResult<bool>> CloseListenKeyAsync(string listenKey, SymbolType type = SymbolType.Main, CancellationToken ct = default)
    {
        // Parameters
        var parameters = new ParameterCollection();
        parameters.Add("listenKey", listenKey);

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
