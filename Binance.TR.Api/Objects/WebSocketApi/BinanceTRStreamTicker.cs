namespace Binance.TR.Api.Objects.WebSocketApi;

/// <summary>
/// Binance TR Stream Ticker
/// </summary>
public record BinanceTRStreamTicker : BinanceTRStreamEvent
{
    /// <summary>
    /// The symbol this data is for
    /// </summary>
    [JsonProperty("s")]
    public string Symbol { get; set; } = string.Empty;

    /// <summary>
    /// The current day close price. This is the latest price for this symbol.
    /// </summary>
    [JsonProperty("c")]
    public decimal LastPrice { get; set; }

    /// <summary>
    /// Todays open price
    /// </summary>
    [JsonProperty("o")]
    public decimal OpenPrice { get; set; }

    /// <summary>
    /// Todays high price
    /// </summary>
    [JsonProperty("h")]
    public decimal HighPrice { get; set; }

    /// <summary>
    /// Todays low price
    /// </summary>
    [JsonProperty("l")]
    public decimal LowPrice { get; set; }

    /// <summary>
    /// Total traded volume in the base asset
    /// </summary>
    [JsonProperty("v")]
    public decimal BaseVolume { get; set; }

    /// <summary>
    /// Total traded volume in the quote asset
    /// </summary>
    [JsonProperty("q")]
    public decimal QuoteVolume { get; set; }
}