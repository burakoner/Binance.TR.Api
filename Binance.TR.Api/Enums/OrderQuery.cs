namespace Binance.TR.Api.Enums;

/// <summary>
/// OrderQueryType
/// </summary>
public enum OrderQuery
{
    /// <summary>
    /// Open
    /// </summary>
    [Map("-1")]
    All = -1,

    /// <summary>
    /// Open
    /// </summary>
    [Map("1")]
    Open = 1,
    
    /// <summary>
    /// Open
    /// </summary>
    [Map("2")]
    History = 2,
}
