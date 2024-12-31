namespace Binance.TR.Api;

/// <summary>
/// BitMart WebSocket Client Options
/// </summary>
public class BinanceTRWebSocketApiOptions : WebSocketApiClientOptions
{
    /// <summary>
    /// Heart Beat Interval
    /// </summary>
    public TimeSpan PingInterval { get; set; }

    /// <summary>
    /// Creates an instance of BitMart WebSocket Client Options
    /// </summary>
    public BinanceTRWebSocketApiOptions()
    {
        // Heartbeat
        this.PingInterval = TimeSpan.FromSeconds(10);
    }
}
