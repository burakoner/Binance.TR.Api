namespace Binance.TR.Api.Objects.WebSocketApi;

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

    [JsonProperty("B")]
    public List<BinanceTRStreamAccountUpdateBalance> Balances { get; set; }
}

public record BinanceTRStreamAccountUpdateBalance
{
    [JsonProperty("a")]
    public string Asset { get; set; }

    [JsonProperty("f")]
    public decimal Free { get; set; }

    [JsonProperty("l")]
    public decimal Locked { get; set; }
}