namespace Binance.TR.Api.Objects.WebSocketApi;

/// <summary>
/// Binance TR Stream Ticker
/// </summary>
public record BinanceTRStreamOrderBookPartial
{
    /// <summary>
    /// The ID of the last update
    /// </summary>
    [JsonProperty("lastUpdateId")]
    public long LastUpdateId { get; set; }

    /// <summary>
    /// The list of bids
    /// </summary>
    public List<BinanceTROrderBookEntry> Bids { get; set; } = [];

    /// <summary>
    /// The list of asks
    /// </summary>
    public List<BinanceTROrderBookEntry> Asks { get; set; } = [];
}