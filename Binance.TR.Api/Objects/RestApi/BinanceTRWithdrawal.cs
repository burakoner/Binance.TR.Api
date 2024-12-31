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

    [JsonProperty("clientId")]
    public string ClientWithdrawalId { get; set; }

    [JsonProperty("asset")]
    public string Asset { get; set; }

    [JsonProperty("network")]
    public string Network { get; set; }

    [JsonProperty("address")]
    public string Address { get; set; }

    [JsonProperty("amount")]
    public decimal Amount { get; set; }

    [JsonProperty("fee")]
    public decimal Fee { get; set; }

    [JsonProperty("txId")]
    public string TransactionId { get; set; }

    [JsonProperty("status")]
    public WithdrawalStatus Status { get; set; }

    [JsonProperty("createTime"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime CreateTime { get; set; }
}
