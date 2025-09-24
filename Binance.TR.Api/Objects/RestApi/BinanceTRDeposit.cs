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
    /// Address Tag
    /// </summary>
    [JsonProperty("addressTag")]
    public string AddressTag { get; set; }

    /// <summary>
    /// Transaction Id
    /// </summary>
    [JsonProperty("txId")]
    public string TransactionId { get; set; }

    /// <summary>
    /// Amount
    /// </summary>
    [JsonProperty("amount")]
    public decimal Amount { get; set; }

    /// <summary>
    /// Status
    /// </summary>
    [JsonProperty("status")]
    public DepositStatus Status { get; set; }

    /// <summary>
    /// Insert Time
    /// </summary>
    [JsonProperty("insertTime"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime InsertTime { get; set; }
}
