namespace Binance.TR.Api.Objects.RestApi;

/// <summary>
/// Binance TR Withdrawal
/// </summary>
public record BinanceTRWithdrawal
{
    /// <summary>
    /// Withdrawal ID
    /// </summary>
    [JsonProperty("id")]
    public long WithdrawalId { get; set; }

    /// <summary>
    /// Client Withdrawal Id
    /// </summary>
    [JsonProperty("clientId")]
    public string ClientWithdrawalId { get; set; }

    /// <summary>
    /// Asset
    /// </summary>
    [JsonProperty("asset")]
    public string Asset { get; set; }

    /// <summary>
    /// Network
    /// </summary>
    [JsonProperty("network")]
    public string Network { get; set; }

    /// <summary>
    /// Address
    /// </summary>
    [JsonProperty("address")]
    public string Address { get; set; }

    /// <summary>
    /// Amount
    /// </summary>
    [JsonProperty("amount")]
    public decimal Amount { get; set; }

    /// <summary>
    /// Fee
    /// </summary>
    [JsonProperty("fee")]
    public decimal Fee { get; set; }

    /// <summary>
    /// Transaction Id
    /// </summary>
    [JsonProperty("txId")]
    public string TransactionId { get; set; }

    /// <summary>
    /// Status
    /// </summary>
    [JsonProperty("status")]
    public WithdrawalStatus Status { get; set; }

    /// <summary>
    /// Create Time
    /// </summary>
    [JsonProperty("createTime"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime CreateTime { get; set; }
}
