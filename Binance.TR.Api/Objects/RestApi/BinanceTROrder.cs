namespace Binance.TR.Api.Objects.RestApi;

/// <summary>
/// Binance TR Order
/// </summary>
public record BinanceTROrder
{
    [JsonProperty("orderId")]
    public long OrderId { get; set; }

    [JsonProperty("clientId")]
    public string ClientOrderId { get; set; }

    [JsonProperty("symbol")]
    public string Symbol { get; set; }

    [JsonProperty("symbolType")]
    public SymbolType SymbolType { get; set; }

    [JsonProperty("side")]
    public OrderSide Side { get; set; }

    [JsonProperty("type")]
    public OrderType Type { get; set; }

    [JsonProperty("price")]
    public decimal? Price { get; set; }

    [JsonProperty("origQty")]
    public decimal? Quantity { get; set; }

    [JsonProperty("origQuoteQty")]
    public decimal? QuoteQuantity { get; set; }

    [JsonProperty("executedQty")]
    public decimal? ExecutedQuantity { get; set; }

    [JsonProperty("executedPrice")]
    public decimal? ExecutedPrice { get; set; }

    [JsonProperty("executedQuoteQty")]
    public decimal? ExecutedQuoteQuantity { get; set; }

    [JsonProperty("timeInForce"), JsonConverter(typeof(MapConverter))]
    public TimeInForce TimeInForce { get; set; }

    [JsonProperty("stopPrice")]
    public decimal? StopPrice { get; set; }

    [JsonProperty("icebergQty")]
    public decimal? IcebergQty { get; set; }

    [JsonProperty("status")]
    public OrderStatus Status { get; set; }

    [JsonProperty("isWorking"), JsonConverter(typeof(BooleanConverter))]
    public bool IsWorking { get; set; }

    [JsonProperty("createTime"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime CreateTime { get; set; }

    [JsonProperty("borderId")]
    public long? BinanceOrderId { get; set; }

    [JsonProperty("borderListId")]
    public long? BinanceOrderListId { get; set; }
}
