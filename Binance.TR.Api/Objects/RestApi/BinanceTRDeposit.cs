namespace Binance.TR.Api.Objects.RestApi;

/// <summary>
/// Binance TR Deposit
/// </summary>
public record BinanceTRDeposit
{
    /// <summary>
    /// Deposit ID
    /// </summary>
    [JsonProperty("id")]
    public long DepositId { get; set; }

    [JsonProperty("asset")]
    public string Asset { get; set; }

    [JsonProperty("network")]
    public string Network { get; set; }

    [JsonProperty("address")]
    public string Address { get; set; }

    [JsonProperty("addressTag")]
    public string AddressTag { get; set; }

    [JsonProperty("txId")]
    public string TransactionId { get; set; }

    [JsonProperty("amount")]
    public decimal Amount { get; set; }

    [JsonProperty("status")]
    public DepositStatus Status { get; set; }

    [JsonProperty("insertTime"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime InsertTime { get; set; }
}
