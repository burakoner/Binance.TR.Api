namespace Binance.TR.Api.Objects.RestApi;

/// <summary>
/// Binance TR Order
/// </summary>
public record BinanceTROrder
{
    /// <summary>
    /// Order Id
    /// </summary>
    [JsonProperty("orderId")]
    public string OrderId { get; set; }

    /// <summary>
    /// Client Order Id
    /// </summary>
    [JsonProperty("clientId")]
    public string ClientOrderId { get; set; }

    /// <summary>
    /// Symbol
    /// </summary>
    [JsonProperty("symbol")]
    public string Symbol { get; set; }

    /// <summary>
    /// Symbol Type
    /// </summary>
    [JsonProperty("symbolType")]
    public SymbolType SymbolType { get; set; }

    /// <summary>
    /// Order Side
    /// </summary>
    [JsonProperty("side")]
    public OrderSide Side { get; set; }

    /// <summary>
    /// Order Type
    /// </summary>
    [JsonProperty("type")]
    public OrderType Type { get; set; }

    /// <summary>
    /// Price
    /// </summary>
    [JsonProperty("price")]
    public decimal? Price { get; set; }

    /// <summary>
    /// Quantity
    /// </summary>
    [JsonProperty("origQty")]
    public decimal? Quantity { get; set; }

    /// <summary>
    /// Quote Quantity
    /// </summary>
    [JsonProperty("origQuoteQty")]
    public decimal? QuoteQuantity { get; set; }

    /// <summary>
    /// Executed Quantity
    /// </summary>
    [JsonProperty("executedQty")]
    public decimal? ExecutedQuantity { get; set; }

    /// <summary>
    /// Executed Price
    /// </summary>
    [JsonProperty("executedPrice")]
    public decimal? ExecutedPrice { get; set; }

    /// <summary>
    /// Executed Quote Quantity
    /// </summary>
    [JsonProperty("executedQuoteQty")]
    public decimal? ExecutedQuoteQuantity { get; set; }

    /// <summary>
    /// Time in Force
    /// </summary>
    [JsonProperty("timeInForce"), JsonConverter(typeof(MapConverter))]
    public TimeInForce TimeInForce { get; set; }

    /// <summary>
    /// Stop Price
    /// </summary>
    [JsonProperty("stopPrice")]
    public decimal? StopPrice { get; set; }

    /// <summary>
    /// Iceberg Quantity
    /// </summary>
    [JsonProperty("icebergQty")]
    public decimal? IcebergQty { get; set; }

    /// <summary>
    /// Status of the order
    /// </summary>
    [JsonProperty("status")]
    public OrderStatus Status { get; set; }

    /// <summary>
    /// Is the order working
    /// </summary>
    [JsonProperty("isWorking"), JsonConverter(typeof(BooleanConverter))]
    public bool IsWorking { get; set; }

    /// <summary>
    /// Creation Time
    /// </summary>
    [JsonProperty("createTime"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// Binance Order Id
    /// </summary>
    [JsonProperty("borderId")]
    public long? BinanceOrderId { get; set; }

    /// <summary>
    /// Binance Order List Id
    /// </summary>
    [JsonProperty("borderListId")]
    public long? BinanceOrderListId { get; set; }

    /// <summary>
    /// Order List Id
    /// </summary>
    [JsonProperty("orderListId")]
    public string OrderListId { get; set; }
}
