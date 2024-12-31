namespace Binance.TR.Api.Objects.RestApi;

/// <summary>
/// Information about a trade
/// </summary>
public record BinanceTRTrade
{
    [JsonIgnore]
    public string Symbol { get; set; }

    /// <summary>
    /// The order id the trade belongs to
    /// </summary>
    [JsonProperty("id")]
    public long OrderId { get; set; }

    /// <summary>
    /// The price of the trade
    /// </summary>
    [JsonProperty("price")]
    public decimal Price { get; set; }

    /// <summary>
    /// The quantity of the trade
    /// </summary>
    [JsonProperty("qty")]
    public decimal Quantity { get; set; }

    /// <summary>
    /// The quote quantity of the trade
    /// </summary>
    [JsonProperty("quoteQty")]
    public decimal QuoteQuantity { get; set; }

    /// <summary>
    /// The time the trade was made
    /// </summary>
    [JsonProperty("time"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime TradeTime { get; set; }

    [JsonProperty("isBuyerMaker")]
    public bool IsBuyerMaker { get; set; }

    [JsonProperty("isBestMatch")]
    public bool IsBestMatch { get; set; }
}
