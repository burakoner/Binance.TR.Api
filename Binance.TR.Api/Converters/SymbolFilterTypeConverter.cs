namespace Binance.TR.Api.Converters;

internal class SymbolFilterTypeConverter : BaseConverter<SymbolFilterType>
{
    public SymbolFilterTypeConverter() : this(true) { }
    public SymbolFilterTypeConverter(bool quotes) : base(quotes) { }

    protected override List<KeyValuePair<SymbolFilterType, string>> Mapping =>
    [
        new(SymbolFilterType.Price, "PRICE_FILTER"),
        new(SymbolFilterType.LotSize, "LOT_SIZE"),
        new(SymbolFilterType.IcebergParts, "ICEBERG_PARTS"),
        new(SymbolFilterType.MarketLotSize, "MARKET_LOT_SIZE"),
        new(SymbolFilterType.TrailingDelta, "TRAILING_DELTA"),
        new(SymbolFilterType.PercentPriceBySide, "PERCENT_PRICE_BY_SIDE"),
        new(SymbolFilterType.Notional, "NOTIONAL"),
        new(SymbolFilterType.MaxNumberOrders, "MAX_NUM_ORDERS"),
        new(SymbolFilterType.MaxNumberAlgorithmicOrders, "MAX_NUM_ALGO_ORDERS"),
    ];
}
