namespace Binance.TR.Api.Enums;

/// <summary>
/// Self trade prevention mode
/// </summary>
public enum SelfTradePreventionMode
{
    /// <summary>
    /// None
    /// </summary>
    [Map("NONE")]
    None = 0,

    /// <summary>
    /// Exire maker
    /// </summary>
    [Map("EXPIRE_MAKER")]
    ExpireMaker = 1,

    /// <summary>
    /// Expire taker
    /// </summary>
    [Map("EXPIRE_TAKER")]
    ExpireTaker = 2,

    /// <summary>
    /// Exire both
    /// </summary>
    [Map("EXPIRE_BOTH")]
    ExpireBoth = 3,
}
