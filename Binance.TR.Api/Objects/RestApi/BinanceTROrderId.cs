namespace Binance.TR.Api.Objects.RestApi;

/// <summary>
/// Response for placing an order
/// </summary>
public record BinanceTROrderId
{
    /// <summary>
    /// Order ID
    /// </summary>
    [JsonProperty("orderId")]
    public long OrderId { get; set; }

    /// <summary>
    /// Create Timestamp
    /// </summary>
    [JsonProperty("createTime"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime CreateTime { get; set; }
}
