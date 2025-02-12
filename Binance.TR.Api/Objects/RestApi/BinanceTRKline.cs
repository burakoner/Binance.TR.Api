﻿namespace Binance.TR.Api.Objects.RestApi;

/// <summary>
/// Candlestick information for symbol
/// </summary>
[JsonConverter(typeof(ArrayConverter))]
public record BinanceTRKline
{
    [JsonIgnore]
    public string Symbol { get; set; }
    
    /// <summary>
    /// The time this candlestick opened
    /// </summary>
    [ArrayProperty(0), JsonConverter(typeof(DateTimeConverter))]
    public DateTime OpenTime { get; set; }

    /// <summary>
    /// The price at which this candlestick opened
    /// </summary>
    [ArrayProperty(1)]
    public decimal Open { get; set; }

    /// <summary>
    /// The highest price in this candlestick
    /// </summary>
    [ArrayProperty(2)]
    public decimal High { get; set; }

    /// <summary>
    /// The lowest price in this candlestick
    /// </summary>
    [ArrayProperty(3)]
    public decimal Low { get; set; }

    /// <summary>
    /// The price at which this candlestick closed
    /// </summary>
    [ArrayProperty(4)]
    public decimal Close { get; set; }

    /// <summary>
    /// The volume traded during this candlestick
    /// </summary>
    [ArrayProperty(5)]
    public decimal BaseVolume { get; set; }

    /// <summary>
    /// The close time of this candlestick
    /// </summary>
    [ArrayProperty(6), JsonConverter(typeof(DateTimeConverter))]
    public DateTime CloseTime { get; set; }

    /// <summary>
    /// The volume traded during this candlestick in the asset form
    /// </summary>
    [ArrayProperty(7)]
    public decimal QuoteVolume { get; set; }

    /// <summary>
    /// The amount of trades in this candlestick
    /// </summary>
    [ArrayProperty(8)]
    public int TradeCount { get; set; }

    /// <summary>
    /// Taker buy base asset volume
    /// </summary>
    [ArrayProperty(9)]
    public decimal TakerBuyBaseVolume { get; set; }

    /// <summary>
    /// Taker buy quote asset volume
    /// </summary>
    [ArrayProperty(10)]
    public decimal TakerBuyQuoteVolume { get; set; }

    /// <summary>
    /// Ignore
    /// </summary>
    [ArrayProperty(11)]
    public decimal? Ignore { get; set; }
}
