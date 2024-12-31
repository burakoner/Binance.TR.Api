namespace Binance.TR.Api.Objects.RestApi;

/// <summary>
/// The order book for a asset
/// </summary>
public record BinanceTROrderBook
{
    /// <summary>
    /// The ID of the last update
    /// </summary>
    [JsonProperty("lastUpdateId")]
    public long LastUpdateId { get; set; }

    /// <summary>
    /// The symbol of the order book 
    /// </summary>
    [JsonProperty("s")]
    public string Symbol { get; set; }

    /// <summary>
    /// The list of bids
    /// </summary>
    public List<BinanceTROrderBookEntry> Bids { get; set; } = [];

    /// <summary>
    /// The list of asks
    /// </summary>
    public List<BinanceTROrderBookEntry> Asks { get; set; } = [];
}
