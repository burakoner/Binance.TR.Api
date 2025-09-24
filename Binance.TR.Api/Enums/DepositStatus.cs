namespace Binance.TR.Api.Enums;

/// <summary>
/// Deposit status
/// </summary>
public enum DepositStatus
{
    /// <summary>
    /// Pending
    /// </summary>
    [Map("0")]
    Pending = 0,

    /// <summary>
    /// Success
    /// </summary>
    [Map("1")]
    Success = 1,
}