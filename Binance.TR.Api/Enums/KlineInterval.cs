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
    OneMinute = 60,

    /// <summary>
    /// 3m
    /// </summary>
    [Map("3m")]
    ThreeMinutes = 180,

    /// <summary>
    /// 5m
    /// </summary>
    [Map("5m")]
    FiveMinutes = 300,

    /// <summary>
    /// 15m
    /// </summary>
    [Map("15m")]
    FifteenMinutes = 900,

    /// <summary>
    /// 30m
    /// </summary>
    [Map("30m")]
    ThirtyMinutes = 1800,

    /// <summary>
    /// 1h
    /// </summary>
    [Map("1h")]
    OneHour = 3600,

    /// <summary>
    /// 2h
    /// </summary>
    [Map("2h")]
    TwoHour = 7200,

    /// <summary>
    /// 4h
    /// </summary>
    [Map("4h")]
    FourHour = 14400,

    /// <summary>
    /// 6h
    /// </summary>
    [Map("6h")]
    SixHour = 21600,

    /// <summary>
    /// 8h
    /// </summary>
    [Map("8h")]
    EightHour = 28800,

    /// <summary>
    /// 12h
    /// </summary>
    [Map("12h")]
    TwelveHour = 43200,

    /// <summary>
    /// 1d
    /// </summary>
    [Map("1d")]
    OneDay = 86400,

    /// <summary>
    /// 3d
    /// </summary>
    [Map("3d")]
    ThreeDay = 259200,

    /// <summary>
    /// 1w
    /// </summary>
    [Map("1w")]
    OneWeek = 604800,

    /// <summary>
    /// 1M
    /// </summary>
    [Map("1M")]
    OneMonth = 2592000
}
