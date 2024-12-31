namespace Binance.TR.Api.Objects.WebSocketApi;

/// <summary>
/// Wrapper for kline information for a symbol
/// </summary>
public record BinanceTRStreamKlineContainer : BinanceTRStreamEvent
{
    /// <summary>
    /// The symbol the data is for
    /// </summary>
    [JsonProperty("s")]
    public string Symbol { get; set; } = string.Empty;

    /// <summary>
    /// The data
    /// </summary>
    [JsonProperty("k")]
    public BinanceTRStreamKline Kline { get; set; } = default!;
}

/// <summary>
/// The kline data
/// </summary>
public record BinanceTRStreamKline
{
    /// <summary>
    /// The open time of this candlestick
    /// </summary>
    [JsonProperty("t"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime OpenTime { get; set; }

    /// <summary>
    /// The close time of this candlestick
    /// </summary>
    [JsonProperty("T"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime CloseTime { get; set; }

    /// <summary>
    /// The symbol this candlestick is for
    /// </summary>
    [JsonProperty("s")]
    public string Symbol { get; set; } = string.Empty;

    /// <summary>
    /// The interval of this candlestick
    /// </summary>
    [JsonProperty("i")]
    public KlineInterval Interval { get; set; }

    /// <summary>
    /// The first trade id in this candlestick
    /// </summary>
    [JsonProperty("f")]
    public long FirstTrade { get; set; }

    /// <summary>
    /// The last trade id in this candlestick
    /// </summary>
    [JsonProperty("L")]
    public long LastTrade { get; set; }

    /// <summary>
    /// The open price of this candlestick
    /// </summary>
    [JsonProperty("o")]
    public decimal Open { get; set; }

    /// <summary>
    /// The close price of this candlestick
    /// </summary>
    [JsonProperty("c")]
    public decimal Close { get; set; }

    /// <summary>
    /// The highest price of this candlestick
    /// </summary>
    [JsonProperty("h")]
    public decimal High { get; set; }

    /// <summary>
    /// The lowest price of this candlestick
    /// </summary>
    [JsonProperty("l")]
    public decimal Low { get; set; }

    [JsonProperty("v")]
    public decimal BaseVolume { get; set; }

    /// <summary>
    /// The amount of trades in this candlestick
    /// </summary>
    [JsonProperty("n")]
    public int TradeCount { get; set; }

    /// <summary>
    /// Boolean indicating whether this candlestick is closed
    /// </summary>
    [JsonProperty("x")]
    public bool Final { get; set; }

    [JsonProperty("q")]
    public decimal QuoteVolume { get; set; }

    [JsonProperty("V")]
    public decimal TakerBuyBaseVolume { get; set; }

    [JsonProperty("Q")]
    public decimal TakerBuyQuoteVolume { get; set; }

    [JsonProperty("B")]
    public decimal? Ignore { get; set; }

    /// <summary>
    /// Casts this object to a <see cref="BinanceTRKline"/> object
    /// </summary>
    /// <returns></returns>
    public BinanceTRKline ToKline()
    {
        return new BinanceTRKline
        {
            Symbol = Symbol,
            OpenTime = OpenTime,
            Open = Open,
            High = High,
            Low = Low,
            Close = Close,
            BaseVolume = BaseVolume,
            CloseTime = CloseTime,
            QuoteVolume = QuoteVolume,
            TradeCount = TradeCount,
            TakerBuyBaseVolume = TakerBuyBaseVolume,
            TakerBuyQuoteVolume = TakerBuyQuoteVolume,
        };
    }
}