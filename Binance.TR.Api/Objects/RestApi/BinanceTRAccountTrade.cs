namespace Binance.TR.Api.Objects.RestApi;

/// <summary>
/// Binance TR Account Trade
/// </summary>
public record BinanceTRAccountTrade
{
    /// <summary>
    /// Trade Id
    /// </summary>
    [JsonProperty("tradeId")]
    public long TradeId { get; set; }

    /// <summary>
    /// Order Id
    /// </summary>
    [JsonProperty("orderId")]
    public long OrderId { get; set; }

    /// <summary>
    /// Symbol
    /// </summary>
    [JsonProperty("symbol")]
    public string Symbol { get; set; }

    /// <summary>
    /// Price
    /// </summary>
    [JsonProperty("price")]
    public decimal Price { get; set; }

    /// <summary>
    /// Quantity
    /// </summary>
    [JsonProperty("qty")]
    public decimal Quantity { get; set; }

    /// <summary>
    /// Quote Quantity
    /// </summary>
    [JsonProperty("quoteQty")]
    public decimal QuoteQuantity { get; set; }

    /// <summary>
    /// Commission
    /// </summary>
    [JsonProperty("commission")]
    public decimal Commission { get; set; }

    /// <summary>
    /// Commission Asset
    /// </summary>
    [JsonProperty("commissionAsset")]
    public string CommissionAsset { get; set; }

    /// <summary>
    /// Is Buyer
    /// </summary>
    [JsonProperty("isBuyer"), JsonConverter(typeof(BooleanConverter))]
    public bool IsBuyer { get; set; }

    /// <summary>
    /// Is Maker
    /// </summary>
    [JsonProperty("isMaker"), JsonConverter(typeof(BooleanConverter))]
    public bool IsMaker { get; set; }

    /// <summary>
    /// Is Best Match
    /// </summary>
    [JsonProperty("isBestMatch"), JsonConverter(typeof(BooleanConverter))]
    public bool IsBestMatch { get; set; }

    /// <summary>
    /// Time
    /// </summary>
    [JsonProperty("time"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime Time { get; set; }
}
