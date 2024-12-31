namespace Binance.TR.Api.Enums;

/// <summary>
/// Filter type
/// </summary>
public enum SymbolFilterType
{
    /// <summary>
    /// Price filter
    /// </summary>
    [Map("PRICE_FILTER")]
    Price,

    /// <summary>
    /// Lot size filter
    /// </summary>
    [Map("LOT_SIZE")]
    LotSize,

    /// <summary>
    /// Max iceberg parts filter
    /// </summary>
    [Map("ICEBERG_PARTS")]
    IcebergParts,

    /// <summary>
    /// Market lot size filter
    /// </summary>
    [Map("MARKET_LOT_SIZE")]
    MarketLotSize,

    /// <summary>
    /// TrailingDelta filter
    /// </summary>
    [Map("TRAILING_DELTA")]
    TrailingDelta,

    /// <summary>
    /// PercentPriceBySide filter
    /// </summary>
    [Map("PERCENT_PRICE_BY_SIDE")]
    PercentPriceBySide,

    /// <summary>
    /// Notional filter
    /// </summary>
    [Map("NOTIONAL")]
    Notional,

    /// <summary>
    /// MaxNumberOrders filter
    /// </summary>
    [Map("MAX_NUM_ORDERS")]
    MaxNumberOrders,

    /// <summary>
    /// Max algo orders filter
    /// </summary>
    [Map("MAX_NUM_ALGO_ORDERS")]
    MaxNumberAlgorithmicOrders,

}
