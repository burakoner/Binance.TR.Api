namespace Binance.TR.Api.Objects.WebSocketApi;

/// <summary>
/// Binance TR Stream Ticker
/// </summary>
public record BinanceTRStreamOrderBookDiff : BinanceTRStreamEvent
{
    /// <summary>
    /// Symbol
    /// </summary>
    [JsonProperty("s")]
    public string Symbol { get; set; }

    /// <summary>
    /// First Update ID
    /// </summary>
    [JsonProperty("U")]
    public long FirstUpdateId { get; set; }

    /// <summary>
    /// Last Update ID
    /// </summary>
    [JsonProperty("u")]
    public long LastUpdateId { get; set; }

    /// <summary>
    /// The list of bids
    /// </summary>
    [JsonProperty("b")]
    public List<BinanceTROrderBookEntry> Bids { get; set; } = [];

    /// <summary>
    /// The list of asks
    /// </summary>
    [JsonProperty("a")]
    public List<BinanceTROrderBookEntry> Asks { get; set; } = [];
}