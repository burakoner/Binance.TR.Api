namespace Binance.TR.Api.Objects.RestApi;

/// <summary>
/// Binance TR Account Information
/// </summary>
public record BinanceTRAccount
{
    /// <summary>
    /// Maker Commission Rate
    /// </summary>
    [JsonProperty("makerCommission")]
    public decimal MakerCommission { get; set; }

    /// <summary>
    /// Taker Commission Rate
    /// </summary>
    [JsonProperty("takerCommission")]
    public decimal TakerCommission { get; set; }

    /// <summary>
    /// Buyer Commission Rate
    /// </summary>
    [JsonProperty("buyerCommission")]
    public decimal BuyerCommission { get; set; }

    /// <summary>
    /// Seller Commission Rate
    /// </summary>
    [JsonProperty("sellerCommission")]
    public decimal SellerCommission { get; set; }

    /// <summary>
    /// Can Trade Flag
    /// </summary>
    [JsonProperty("canTrade"), JsonConverter(typeof(BooleanConverter))]
    public bool CanTrade { get; set; }

    /// <summary>
    /// Can Deposit Flag
    /// </summary>
    [JsonProperty("canDeposit"), JsonConverter(typeof(BooleanConverter))]
    public bool CanDeposit { get; set; }

    /// <summary>
    /// Can Withdraw Flag
    /// </summary>
    [JsonProperty("canWithdraw"), JsonConverter(typeof(BooleanConverter))]
    public bool CanWithdraw { get; set; }

    /// <summary>
    /// Status of the account
    /// </summary>
    [JsonProperty("status"), JsonConverter(typeof(BooleanConverter))]
    public bool Status { get; set; }

    /// <summary>
    /// Balances of the account
    /// </summary>
    [JsonProperty("accountAssets")]
    public List<BinanceTRAccountBalance> Balances { get; set; }
}

/// <summary>
/// Binance TR Account Asset Balance
/// </summary>
public record BinanceTRAccountBalance
{
    /// <summary>
    /// Asset Symbol
    /// </summary>
    [JsonProperty("asset")]
    public string Asset { get; set; }

    /// <summary>
    /// Free Balance
    /// </summary>
    [JsonProperty("free")]
    public decimal Free { get; set; }

    /// <summary>
    /// Locked Balance
    /// </summary>
    [JsonProperty("locked")]
    public decimal Locked { get; set; }
}