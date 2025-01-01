namespace Binance.TR.Api.Enums;

/// <summary>
/// The reason the order was rejected
/// </summary>
public enum OrderRejectReason
{
    /// <summary>
    /// Not rejected
    /// </summary>
    [Map("NONE")]
    None = 0,

    /// <summary>
    /// Unknown instrument
    /// </summary>
    [Map("UNKNOWN_INSTRUMENT")]
    UnknownInstrument = 1,

    /// <summary>
    /// Closed market
    /// </summary>
    [Map("MARKET_CLOSED")]
    MarketClosed = 2,

    /// <summary>
    /// Quantity out of bounds
    /// </summary>
    [Map("PRICE_QTY_EXCEED_HARD_LIMITS")]
    PriceQuantityExceedsHardLimits = 3,

    /// <summary>
    /// Unknown order
    /// </summary>
    [Map("UNKNOWN_ORDER")]
    UnknownOrder = 4,

    /// <summary>
    /// Duplicate
    /// </summary>
    [Map("DUPLICATE_ORDER")]
    DuplicateOrder = 5,

    /// <summary>
    /// Unkown account
    /// </summary>
    [Map("UNKNOWN_ACCOUNT")]
    UnknownAccount = 6,

    /// <summary>
    /// Not enough balance
    /// </summary>
    [Map("INSUFFICIENT_BALANCE")]
    InsufficientBalance = 7,

    /// <summary>
    /// Account not active
    /// </summary>
    [Map("ACCOUNT_INACTIVE")]
    AccountInactive = 8,

    /// <summary>
    /// Cannot settle
    /// </summary>
    [Map("ACCOUNT_CANNOT_SETTLE")]
    AccountCannotSettle = 9,

    /// <summary>
    /// Stop price would trigger immediately
    /// </summary>
    [Map("STOP_PRICE_WOULD_TRIGGER_IMMEDIATELY")]
    StopPriceWouldTrigger = 10
}