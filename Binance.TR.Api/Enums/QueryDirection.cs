namespace Binance.TR.Api.Enums;

/// <summary>
/// QueryDirection
/// </summary>
public enum QueryDirection
{
    /// <summary>
    /// Open
    /// </summary>
    [Map("prev")]
    Previous = -1,

    /// <summary>
    /// Open
    /// </summary>
    [Map("next")]
    Next = 1,
}
