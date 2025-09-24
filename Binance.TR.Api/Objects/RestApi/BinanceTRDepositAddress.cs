namespace Binance.TR.Api.Objects.RestApi;

/// <summary>
/// Binance TR Deposit Address
/// </summary>
public record BinanceTRDepositAddress
{
    /// <summary>
    /// Asset
    /// </summary>
    [JsonProperty("asset")]
    public string Asset { get; set; }

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
}
