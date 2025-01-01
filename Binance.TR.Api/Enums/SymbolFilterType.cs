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
    Price = 1,

    /// <summary>
    /// Lot size filter
    /// </summary>
    [Map("LOT_SIZE")]
    LotSize = 2,

    /// <summary>
    /// Max iceberg parts filter
    /// </summary>
    [Map("ICEBERG_PARTS")]
    IcebergParts = 3,

    /// <summary>
    /// Market lot size filter
    /// </summary>
    [Map("MARKET_LOT_SIZE")]
    MarketLotSize = 4,

    /// <summary>
    /// TrailingDelta filter
    /// </summary>
    [Map("TRAILING_DELTA")]
    TrailingDelta = 5,

    /// <summary>
    /// PercentPriceBySide filter
    /// </summary>
    [Map("PERCENT_PRICE_BY_SIDE")]
    PercentPriceBySide = 6,

    /// <summary>
    /// Notional filter
    /// </summary>
    [Map("NOTIONAL")]
    Notional = 7,

    /// <summary>
    /// MaxNumberOrders filter
    /// </summary>
    [Map("MAX_NUM_ORDERS")]
    MaxNumberOrders = 8,

    /// <summary>
    /// Max algo orders filter
    /// </summary>
    [Map("MAX_NUM_ALGO_ORDERS")]
    MaxNumberAlgorithmicOrders = 9,

}
