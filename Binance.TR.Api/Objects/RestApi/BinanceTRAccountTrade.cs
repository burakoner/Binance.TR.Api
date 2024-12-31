namespace Binance.TR.Api.Objects.RestApi;

/// <summary>
/// Binance TR Account Trade
/// </summary>
public record BinanceTRAccountTrade
{
    [JsonProperty("tradeId")]
    public long TradeId { get; set; }

    [JsonProperty("orderId")]
    public long OrderId { get; set; }

    [JsonProperty("symbol")]
    public string Symbol { get; set; }

    [JsonProperty("price")]
    public decimal Price { get; set; }

    [JsonProperty("qty")]
    public decimal Quantity { get; set; }

    [JsonProperty("quoteQty")]
    public decimal QuoteQuantity { get; set; }

    [JsonProperty("commission")]
    public decimal Commission { get; set; }

    [JsonProperty("commissionAsset")]
    public string CommissionAsset { get; set; }

    [JsonProperty("isBuyer"), JsonConverter(typeof(BooleanConverter))]
    public bool IsBuyer { get; set; }

    [JsonProperty("isMaker"), JsonConverter(typeof(BooleanConverter))]
    public bool IsMaker { get; set; }

    [JsonProperty("isBestMatch"), JsonConverter(typeof(BooleanConverter))]
    public bool IsBestMatch { get; set; }

    [JsonProperty("time"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime Time { get; set; }
}
