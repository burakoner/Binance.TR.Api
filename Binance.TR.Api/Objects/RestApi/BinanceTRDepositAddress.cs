namespace Binance.TR.Api.Objects.RestApi;

/// <summary>
/// Binance TR Deposit Address
/// </summary>
public record BinanceTRDepositAddress
{
    [JsonProperty("asset")]
    public string Asset { get; set; }

    [JsonProperty("address")]
    public string Address { get; set; }

    [JsonProperty("addressTag")]
    public string AddressTag { get; set; }
}
