namespace Binance.TR.Api.Objects.RestApi;

/// <summary>
/// Binance TR Symbol Filter
/// </summary>
[JsonConverter(typeof(SymbolFilterConverter))]
public record BinanceTRSymbolFilter
{
    /// <summary>
    /// Apply the filter to market orders.
    /// </summary>
    [JsonProperty("applyToMarket")]
    public bool ApplyToMarket { get; set; }

    /// <summary>
    /// Filter Type
    /// </summary>
    [JsonProperty("filterType")]
    public SymbolFilterType FilterType { get; set; }
}

/// <summary>
/// Binance TR Symbol Price Filter
/// </summary>
public record BinanceSymbolPriceFilter : BinanceTRSymbolFilter
{
    /// <summary>
    /// Minimal price for an order
    /// </summary>
    [JsonProperty("minPrice")]
    public decimal MinPrice { get; set; }

    /// <summary>
    /// Maximal price for an order
    /// </summary>
    [JsonProperty("maxPrice")]
    public decimal MaxPrice { get; set; }

    /// <summary>
    /// Tick Size
    /// </summary>
    [JsonProperty("tickSize")]
    public decimal TickSize { get; set; }
}

/// <summary>
/// Binance TR Symbol Lot Size Filter
/// </summary>
public record BinanceSymbolLotSizeFilter : BinanceTRSymbolFilter
{
    /// <summary>
    /// Minimal quantity for an order
    /// </summary>
    [JsonProperty("minQty")]
    public decimal MinQuantity { get; set; }

    /// <summary>
    /// Maximal quantity for an order
    /// </summary>
    [JsonProperty("maxQty")]
    public decimal MaxQuantity { get; set; }

    /// <summary>
    /// Step Size
    /// </summary>
    [JsonProperty("stepSize")]
    public decimal StepSize { get; set; }
}

/// <summary>
/// Binance TR Symbol Min Notional Filter
/// </summary>
public record BinanceSymbolIcebergPartsFilter : BinanceTRSymbolFilter
{
    /// <summary>
    /// Limit for the number of iceberg parts
    /// </summary>
    [JsonProperty("limit")]
    public int Limit { get; set; }
}

/// <summary>
/// Binance TR Symbol Market Lot Size Filter
/// </summary>
public record BinanceSymbolMarketLotSizeFilter : BinanceTRSymbolFilter
{
    /// <summary>
    /// Minimal quantity for an order
    /// </summary>
    [JsonProperty("minQty")]
    public decimal MinQuantity { get; set; }

    /// <summary>
    /// Maximal quantity for an order
    /// </summary>
    [JsonProperty("maxQty")]
    public decimal MaxQuantity { get; set; }

    /// <summary>
    /// Step Size
    /// </summary>
    [JsonProperty("stepSize")]
    public decimal StepSize { get; set; }
}

/// <summary>
/// Binance TR Symbol Trailing Delta Filter
/// </summary>
public record BinanceSymbolTrailingDeltaFilter : BinanceTRSymbolFilter
{
}

/// <summary>
/// Binance TR Symbol Percent Price By Side Filter
/// </summary>
public record BinanceSymbolPercentPriceBySideFilter : BinanceTRSymbolFilter
{
    /// <summary>
    /// Bid Multiplier Up
    /// </summary>
    [JsonProperty("bidMultiplierUp")]
    public decimal BidMultiplierUp { get; set; }

    /// <summary>
    /// Bid Multiplier Down
    /// </summary>
    [JsonProperty("bidMultiplierDown")]
    public decimal BidMultiplierDown { get; set; }

    /// <summary>
    /// Ask Multiplier Up
    /// </summary>
    [JsonProperty("askMultiplierUp")]
    public decimal AskMultiplierUp { get; set; }

    /// <summary>
    /// Ask Multiplier Down
    /// </summary>
    [JsonProperty("askMultiplierDown")]
    public decimal AskMultiplierDown { get; set; }

    /// <summary>
    /// Average Price Minutes
    /// </summary>
    [JsonProperty("avgPriceMins")]
    public int AveragePriceMinutes { get; set; }
}

/// <summary>
/// Binance TR Symbol Notional Filter
/// </summary>
public record BinanceSymbolNotionalFilter : BinanceTRSymbolFilter
{
    /// <summary>
    /// The minimal total size of an order. This is calculated by Price * Quantity.
    /// </summary>
    [JsonProperty("minNotional")]
    public decimal MinNotional { get; set; }

    /// <summary>
    /// The amount of minutes the average price of trades is calculated over for market orders. 0 means the last price is used
    /// </summary>
    [JsonProperty("avgPriceMins")]
    public int AveragePriceMinutes { get; set; }
}

/// <summary>
/// Binance TR Symbol Max Orders Filter
/// </summary>
public record BinanceSymbolMaxOrdersFilter : BinanceTRSymbolFilter
{
}

/// <summary>
/// Binance TR Symbol Max Algorithmic Orders Filter
/// </summary>
public record BinanceSymbolMaxAlgorithmicOrdersFilter : BinanceTRSymbolFilter
{
    /// <summary>
    /// Maximum number of algorithmic orders a user can have open on a symbol
    /// </summary>
    [JsonProperty("maxNumAlgoOrders")]
    public int MaxNumberAlgorithmicOrders { get; set; }
}

/// <summary>
/// Binance TR Symbol Max Order Lists Filter
/// </summary>
public record BinanceSymbolMaxOrderListsFilter : BinanceTRSymbolFilter
{
}

/// <summary>
/// Binance TR Symbol Max Order Amends Filter
/// </summary>
public record BinanceSymbolMaxOrderAmendsFilter : BinanceTRSymbolFilter
{
}

