namespace Binance.TR.Api.Objects.Internal;

internal class BinanceTRSocketRequest
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("method")]
    public string Method { get; set; } = "";

    [JsonProperty("params")]
    public string[] Parameters { get; set; } = [];
}
