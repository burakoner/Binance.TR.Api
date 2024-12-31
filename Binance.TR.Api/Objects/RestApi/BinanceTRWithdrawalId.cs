namespace Binance.TR.Api.Objects.RestApi;

/// <summary>
/// Binance TR Withdrawal Id
/// </summary>
public record BinanceTRWithdrawalId
{
    /// <summary>
    /// Withdrawal ID
    /// </summary>
    [JsonProperty("withdrawId")]
    public long WithdrawalId { get; set; }
}
