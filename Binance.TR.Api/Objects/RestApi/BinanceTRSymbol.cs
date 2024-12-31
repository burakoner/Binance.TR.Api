namespace Binance.TR.Api.Objects.RestApi;

public record BinanceTRSymbol
{
    [JsonProperty("type")]
    public SymbolType Type { get; set; }

    [JsonProperty("symbol")]
    public string Symbol { get; set; }

    [JsonProperty("baseAsset")]
    public string BaseAsset { get; set; }

    [JsonProperty("basePrecision")]
    public int BaseAssetPrecision { get; set; }

    [JsonProperty("quoteAsset")]
    public string QuoteAsset { get; set; }

    [JsonProperty("quotePrecision")]
    public int QuoteAssetPrecision { get; set; }

    [JsonProperty("orderTypes")]
    public List<OrderType> OrderTypes { get; set; } = [];

    [JsonProperty("icebergEnable"), JsonConverter(typeof(BooleanConverter))]
    public bool IcebergEnable { get; set; }

    [JsonProperty("ocoEnable"), JsonConverter(typeof(BooleanConverter))]
    public bool OcoEnable { get; set; }

    [JsonProperty("spotTradingEnable"), JsonConverter(typeof(BooleanConverter))]
    public bool SpotTradingEnable { get; set; }

    [JsonProperty("marginTradingEnable"), JsonConverter(typeof(BooleanConverter))]
    public bool MarginTradingEnable { get; set; }

    [JsonProperty("filters")]
    public List<BinanceTRSymbolFilter> Filters { get; set; } = [];

    [JsonIgnore]
    public BinanceSymbolPriceFilter PriceFilter => Filters.OfType<BinanceSymbolPriceFilter>().FirstOrDefault();

    [JsonIgnore]
    public BinanceSymbolPercentPriceBySideFilter PricePercentFilter => Filters.OfType<BinanceSymbolPercentPriceBySideFilter>().FirstOrDefault();

    [JsonIgnore]
    public BinanceSymbolLotSizeFilter LotSizeFilter => Filters.OfType<BinanceSymbolLotSizeFilter>().FirstOrDefault();

    [JsonIgnore]
    public BinanceSymbolNotionalFilter MinNotionalFilter => Filters.OfType<BinanceSymbolNotionalFilter>().FirstOrDefault();

    [JsonIgnore]
    public BinanceSymbolIcebergPartsFilter IceBergPartsFilter => Filters.OfType<BinanceSymbolIcebergPartsFilter>().FirstOrDefault();

    [JsonIgnore]
    public BinanceSymbolMarketLotSizeFilter MarketLotSizeFilter => Filters.OfType<BinanceSymbolMarketLotSizeFilter>().FirstOrDefault();

    [JsonIgnore]
    public BinanceSymbolMaxAlgorithmicOrdersFilter MaxAlgorithmicOrdersFilter => Filters.OfType<BinanceSymbolMaxAlgorithmicOrdersFilter>().FirstOrDefault();
}
