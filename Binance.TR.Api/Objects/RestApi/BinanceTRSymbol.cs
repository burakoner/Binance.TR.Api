namespace Binance.TR.Api.Objects.RestApi;

/// <summary>
/// Binance TR Symbol Information
/// </summary>
public record BinanceTRSymbol
{
    /// <summary>
    /// Type
    /// </summary>
    [JsonProperty("type")]
    public SymbolType Type { get; set; }

    /// <summary>
    /// Symbol
    /// </summary>
    [JsonProperty("symbol")]
    public string Symbol { get; set; }

    /// <summary>
    /// Base Asset
    /// </summary>
    [JsonProperty("baseAsset")]
    public string BaseAsset { get; set; }

    /// <summary>
    /// Base Asset Precision
    /// </summary>
    [JsonProperty("basePrecision")]
    public int BaseAssetPrecision { get; set; }

    /// <summary>
    /// Quote Asset
    /// </summary>
    [JsonProperty("quoteAsset")]
    public string QuoteAsset { get; set; }

    /// <summary>
    /// Quote Asset Precision
    /// </summary>
    [JsonProperty("quotePrecision")]
    public int QuoteAssetPrecision { get; set; }

    /// <summary>
    /// Order Types
    /// </summary>
    [JsonProperty("orderTypes")]
    public List<OrderType> OrderTypes { get; set; } = [];

    /// <summary>
    /// Is Iceberg Orders Enabled
    /// </summary>
    [JsonProperty("icebergEnable"), JsonConverter(typeof(BooleanConverter))]
    public bool IcebergEnable { get; set; }

    /// <summary>
    /// Is OCO Orders Enabled
    /// </summary>
    [JsonProperty("ocoEnable"), JsonConverter(typeof(BooleanConverter))]
    public bool OcoEnable { get; set; }

    /// <summary>
    /// Is Spot Trading Enabled
    /// </summary>
    [JsonProperty("spotTradingEnable"), JsonConverter(typeof(BooleanConverter))]
    public bool SpotTradingEnable { get; set; }

    /// <summary>
    /// Is Margin Trading Enabled
    /// </summary>
    [JsonProperty("marginTradingEnable"), JsonConverter(typeof(BooleanConverter))]
    public bool MarginTradingEnable { get; set; }

    /// <summary>
    /// Filters
    /// </summary>
    [JsonProperty("filters")]
    public List<BinanceTRSymbolFilter> Filters { get; set; } = [];

    /// <summary>
    /// Price Filter
    /// </summary>
    [JsonIgnore]
    public BinanceSymbolPriceFilter PriceFilter => Filters.OfType<BinanceSymbolPriceFilter>().FirstOrDefault();

    /// <summary>
    /// Price Percent Filter
    /// </summary>
    [JsonIgnore]
    public BinanceSymbolPercentPriceBySideFilter PricePercentFilter => Filters.OfType<BinanceSymbolPercentPriceBySideFilter>().FirstOrDefault();

    /// <summary>
    /// Lot Size Filter
    /// </summary>
    [JsonIgnore]
    public BinanceSymbolLotSizeFilter LotSizeFilter => Filters.OfType<BinanceSymbolLotSizeFilter>().FirstOrDefault();

    /// <summary>
    /// Min Notional Filter
    /// </summary>
    [JsonIgnore]
    public BinanceSymbolNotionalFilter MinNotionalFilter => Filters.OfType<BinanceSymbolNotionalFilter>().FirstOrDefault();

    /// <summary>
    /// Iceberg Parts Filter
    /// </summary>
    [JsonIgnore]
    public BinanceSymbolIcebergPartsFilter IceBergPartsFilter => Filters.OfType<BinanceSymbolIcebergPartsFilter>().FirstOrDefault();

    /// <summary>
    /// Market Lot Size Filter
    /// </summary>
    [JsonIgnore]
    public BinanceSymbolMarketLotSizeFilter MarketLotSizeFilter => Filters.OfType<BinanceSymbolMarketLotSizeFilter>().FirstOrDefault();

    /// <summary>
    /// Max Algorithmic Orders Filter
    /// </summary>
    [JsonIgnore]
    public BinanceSymbolMaxAlgorithmicOrdersFilter MaxAlgorithmicOrdersFilter => Filters.OfType<BinanceSymbolMaxAlgorithmicOrdersFilter>().FirstOrDefault();

    /// <summary>
    /// Max Orders Filter
    /// </summary>
    [JsonIgnore]
    public BinanceSymbolMaxOrderListsFilter MaxOrderListsFilter => Filters.OfType<BinanceSymbolMaxOrderListsFilter>().FirstOrDefault();

    /// <summary>
    /// Max Order Amends Filter
    /// </summary>
    [JsonIgnore]
    public BinanceSymbolMaxOrderAmendsFilter MaxOrderAmendsFilter => Filters.OfType<BinanceSymbolMaxOrderAmendsFilter>().FirstOrDefault();
}
