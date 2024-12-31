namespace Binance.TR.Api.Objects.RestApi;

/// <summary>
/// Binance TR Account Information
/// </summary>
public record BinanceTRAccount
{
    [JsonProperty("makerCommission")]
    public decimal MakerCommission { get; set; }

    [JsonProperty("takerCommission")]
    public decimal TakerCommission { get; set; }

    [JsonProperty("buyerCommission")]
    public decimal BuyerCommission { get; set; }

    [JsonProperty("sellerCommission")]
    public decimal SellerCommission { get; set; }

    [JsonProperty("canTrade"), JsonConverter(typeof(BooleanConverter))]
    public bool CanTrade { get; set; }

    [JsonProperty("canDeposit"), JsonConverter(typeof(BooleanConverter))]
    public bool CanDeposit { get; set; }

    [JsonProperty("canWithdraw"), JsonConverter(typeof(BooleanConverter))]
    public bool CanWithdraw { get; set; }

    [JsonProperty("status"), JsonConverter(typeof(BooleanConverter))]
    public bool Status { get; set; }

    [JsonProperty("accountAssets")]
    public List<BinanceTRAccountAsset> Assets { get; set; }
}

/// <summary>
/// Binance TR Account Asset Balance
/// </summary>
public record BinanceTRAccountAsset
{
    [JsonProperty("asset")]
    public string Asset { get; set; }

    [JsonProperty("free")]
    public decimal Free { get; set; }

    [JsonProperty("locked")]
    public decimal Locked { get; set; }
}