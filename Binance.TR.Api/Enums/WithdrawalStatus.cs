namespace Binance.TR.Api.Enums;

/// <summary>
/// Withdrawal status
/// </summary>
public enum WithdrawalStatus
{
    /// <summary>
    /// Email Sent
    /// </summary>
    [Map("0")]
    EmailSent = 0,

    /// <summary>
    /// Cancelled
    /// </summary>
    [Map("1")]
    Cancelled = 1,

    /// <summary>
    /// Awaiting Approval
    /// </summary>
    [Map("2")]
    AwaitingApproval = 2,

    /// <summary>
    /// Rejected
    /// </summary>
    [Map("3")]
    Rejected = 3,

    /// <summary>
    /// Processing
    /// </summary>
    [Map("4")]
    Processing = 4,

    /// <summary>
    /// Failure
    /// </summary>
    [Map("5")]
    Failure = 5,

    /// <summary>
    /// Completed
    /// </summary>
    [Map("10")]
    Completed = 10,
}