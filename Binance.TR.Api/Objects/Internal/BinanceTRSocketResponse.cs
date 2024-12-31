namespace Binance.TR.Api.Objects.Internal;

internal class BinanceTRSocketResponse
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("result")]
    public object Result { get; set; }
}
