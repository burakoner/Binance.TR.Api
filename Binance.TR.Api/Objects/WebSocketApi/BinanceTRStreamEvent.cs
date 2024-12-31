﻿namespace Binance.TR.Api.Objects.WebSocketApi;

/// <summary>
/// A event received by a Binance websocket
/// </summary>
public record BinanceTRStreamEvent
{
    /// <summary>
    /// The type of the event
    /// </summary>
    [JsonProperty("e")]
    public string Event { get; set; } = string.Empty;

    /// <summary>
    /// The time the event happened
    /// </summary>
    [JsonProperty("E"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime EventTime { get; set; }
}
