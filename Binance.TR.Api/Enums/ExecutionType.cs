namespace Binance.TR.Api.Enums;

/// <summary>
/// The type of execution
/// </summary>
public enum ExecutionType
{
    /// <summary>
    /// New
    /// </summary>
    [Map("NEW")]
    New = 1,

    /// <summary>
    /// Canceled
    /// </summary>
    [Map("CANCELED")]
    Canceled = 2,

    /// <summary>
    /// Replaced
    /// </summary>
    [Map("REPLACED")]
    Replaced = 3,

    /// <summary>
    /// Rejected
    /// </summary>
    [Map("REJECTED")]
    Rejected = 4,

    /// <summary>
    /// Trade
    /// </summary>
    [Map("TRADE")]
    Trade = 5,

    /// <summary>
    /// Expired
    /// </summary>
    [Map("EXPIRED")]
    Expired = 6,

    /// <summary>
    /// Amendment
    /// </summary>
    [Map("AMENDMENT")]
    Amendment = 7,

    /// <summary>
    /// Self trade prevented
    /// </summary>
    [Map("TRADE_PREVENTION")]
    TradePrevention = 8
}
