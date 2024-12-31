namespace Binance.TR.Api.Enums;

/// <summary>
/// The status of an orderн
/// </summary>
public enum OrderStatus
{
    /// <summary>
    /// SYSTEM_PROCESSING
    /// </summary>
    [Map("-2")]
    SystemProcessing = -2,

    /// <summary>
    /// Order is new
    /// </summary>
    [Map("0")]
    New = 0,

    /// <summary>
    /// Order is partly filled, still has quantity left to fill
    /// </summary>
    [Map("1")]
    PartiallyFilled = 1,

    /// <summary>
    /// The order has been filled and completed
    /// </summary>
    [Map("2")]
    Filled = 2,

    /// <summary>
    /// The order has been canceled
    /// </summary>
    [Map("3")]
    Canceled = 3,

    /// <summary>
    /// The order is in the process of being canceled  (currently unused)
    /// </summary>
    [Map("4")]
    PendingCancel = 4,

    /// <summary>
    /// The order has been rejected
    /// </summary>
    [Map("5")]
    Rejected = 5,

    /// <summary>
    /// The order has expired
    /// </summary>
    [Map("6")]
    Expired = 6,
}
