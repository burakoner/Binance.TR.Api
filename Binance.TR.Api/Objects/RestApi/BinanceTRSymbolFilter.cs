namespace Binance.TR.Api.Objects.RestApi;

[JsonConverter(typeof(SymbolFilterConverter))]
public record BinanceTRSymbolFilter
{
    [JsonProperty("applyToMarket")]
    public bool ApplyToMarket { get; set; }

    [JsonProperty("filterType")]
    public SymbolFilterType FilterType { get; set; }
}

public record BinanceSymbolPriceFilter : BinanceTRSymbolFilter
{
    [JsonProperty("minPrice")]
    public decimal MinPrice { get; set; }

    [JsonProperty("maxPrice")]
    public decimal MaxPrice { get; set; }

    [JsonProperty("tickSize")]
    public decimal TickSize { get; set; }
}

public record BinanceSymbolLotSizeFilter : BinanceTRSymbolFilter
{
    [JsonProperty("minQty")]
    public decimal MinQuantity { get; set; }

    [JsonProperty("maxQty")]
    public decimal MaxQuantity { get; set; }

    [JsonProperty("stepSize")]
    public decimal StepSize { get; set; }
}

public record BinanceSymbolIcebergPartsFilter : BinanceTRSymbolFilter
{
    [JsonProperty("limit")]
    public int Limit { get; set; }
}

public record BinanceSymbolMarketLotSizeFilter : BinanceTRSymbolFilter
{
    [JsonProperty("minQty")]
    public decimal MinQuantity { get; set; }

    [JsonProperty("maxQty")]
    public decimal MaxQuantity { get; set; }

    [JsonProperty("stepSize")]
    public decimal StepSize { get; set; }
}

public record BinanceSymbolTrailingDeltaFilter : BinanceTRSymbolFilter
{
}

public record BinanceSymbolPercentPriceBySideFilter : BinanceTRSymbolFilter
{
    [JsonProperty("bidMultiplierUp")]
    public decimal BidMultiplierUp { get; set; }

    [JsonProperty("bidMultiplierDown")]
    public decimal BidMultiplierDown { get; set; }

    [JsonProperty("askMultiplierUp")]
    public decimal AskMultiplierUp { get; set; }

    [JsonProperty("askMultiplierDown")]
    public decimal AskMultiplierDown { get; set; }

    [JsonProperty("avgPriceMins")]
    public int AveragePriceMinutes { get; set; }
}

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

public record BinanceSymbolMaxOrdersFilter : BinanceTRSymbolFilter
{
}

public record BinanceSymbolMaxAlgorithmicOrdersFilter : BinanceTRSymbolFilter
{
    [JsonProperty("maxNumAlgoOrders")]
    public int MaxNumberAlgorithmicOrders { get; set; }
}

public record BinanceSymbolMaxOrderListsFilter : BinanceTRSymbolFilter
{
}



public record BinanceSymbolMaxOrderAmendsFilter : BinanceTRSymbolFilter
{
}

