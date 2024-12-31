namespace Binance.TR.Api;

/// <summary>
/// Binance.TR Api Addresses
/// </summary>
public class BinanceTRAddress
{
    /// <summary>
    /// Rest Api Address
    /// </summary>
    public string MeRestApiAddress { get; set; } = string.Empty;

    /// <summary>
    /// Rest Api Address
    /// </summary>
    public string TrRestApiAddress { get; set; } = string.Empty;

    /// <summary>
    /// 2meta Rest Api Address
    /// </summary>
    public string AppRestApiAddress { get; set; } = string.Empty;

    /// <summary>
    /// WebSocket Main Address
    /// </summary>
    public string WebSocketMainAddress { get; set; } = string.Empty;

    /// <summary>
    /// WebSocket Next Address
    /// </summary>
    public string WebSocketNextAddress { get; set; } = string.Empty;

    /// <summary>
    /// Default Environment Endpoints
    /// </summary>
    public static BinanceTRAddress Default = new()
    {
        MeRestApiAddress = "https://api.binance.me",
        TrRestApiAddress = "https://www.binance.tr",
        AppRestApiAddress = "https://cloudme-tr.2meta.app",
        WebSocketMainAddress = "wss://stream-cloud.binance.tr/ws",
        WebSocketNextAddress = "wss://www.binance.tr",
    };
}

public enum BinanceDataCenter
{
    ME,
    TR,
    App,
    WebSocketMain,
    WebSocketNext,
}