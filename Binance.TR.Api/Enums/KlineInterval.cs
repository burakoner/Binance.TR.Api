namespace Binance.TR.Api.Enums;

/// <summary>
/// The interval for the kline
/// </summary>
public enum KlineInterval
{
    /// <summary>
    /// 1m
    /// </summary>
    [Map("1m")]
    OneMinute,

    /// <summary>
    /// 3m
    /// </summary>
    [Map("3m")]
    ThreeMinutes,

    /// <summary>
    /// 5m
    /// </summary>
    [Map("5m")]
    FiveMinutes,

    /// <summary>
    /// 15m
    /// </summary>
    [Map("15m")]
    FifteenMinutes,

    /// <summary>
    /// 30m
    /// </summary>
    [Map("30m")]
    ThirtyMinutes,

    /// <summary>
    /// 1h
    /// </summary>
    [Map("1h")]
    OneHour,

    /// <summary>
    /// 2h
    /// </summary>
    [Map("2h")]
    TwoHour,

    /// <summary>
    /// 4h
    /// </summary>
    [Map("4h")]
    FourHour,

    /// <summary>
    /// 6h
    /// </summary>
    [Map("6h")]
    SixHour,

    /// <summary>
    /// 8h
    /// </summary>
    [Map("8h")]
    EightHour,

    /// <summary>
    /// 12h
    /// </summary>
    [Map("12h")]
    TwelveHour,

    /// <summary>
    /// 1d
    /// </summary>
    [Map("1d")]
    OneDay,

    /// <summary>
    /// 3d
    /// </summary>
    [Map("3d")]
    ThreeDay,

    /// <summary>
    /// 1w
    /// </summary>
    [Map("1w")]
    OneWeek,

    /// <summary>
    /// 1M
    /// </summary>
    [Map("1M")]
    OneMonth
}
