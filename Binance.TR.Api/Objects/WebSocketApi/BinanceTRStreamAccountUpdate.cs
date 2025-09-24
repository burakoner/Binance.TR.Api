namespace Binance.TR.Api.Objects.WebSocketApi;

/// <summary>
/// Binance TR Account Update Stream Event
/// </summary>
public record BinanceTRStreamAccountUpdate : BinanceTRStreamEvent
{
    // Bu alanlar API Docs'ta olmasına rağmen dönen yanıtta yoklar.

    /*
    [JsonProperty("m")]
    public decimal MakerCommission { get; set; }

    [JsonProperty("t")]
    public decimal TakerCommission { get; set; }

    [JsonProperty("b")]
    public decimal BuyerCommission { get; set; }

    [JsonProperty("s")]
    public decimal SellerCommission { get; set; }

    [JsonProperty("T"), JsonConverter(typeof(BooleanConverter))]
    public bool CanTrade { get; set; }

    [JsonProperty("D"), JsonConverter(typeof(BooleanConverter))]
    public bool CanDeposit { get; set; }

    [JsonProperty("W"), JsonConverter(typeof(BooleanConverter))]
    public bool CanWithdraw { get; set; }
    */

    //[JsonProperty("status"), JsonConverter(typeof(BooleanConverter))]
    //public bool Status { get; set; }

    /// <summary>
    /// Balances
    /// </summary>
    [JsonProperty("B")]
    public List<BinanceTRStreamAccountUpdateBalance> Balances { get; set; }
}

/// <summary>
/// Binance TR Account Update Balance
/// </summary>
public record BinanceTRStreamAccountUpdateBalance
{
    /// <summary>
    /// Asset
    /// </summary>
    [JsonProperty("a")]
    public string Asset { get; set; }

    /// <summary>
    /// Free
    /// </summary>
    [JsonProperty("f")]
    public decimal Free { get; set; }

    /// <summary>
    /// Locked
    /// </summary>
    [JsonProperty("l")]
    public decimal Locked { get; set; }
}