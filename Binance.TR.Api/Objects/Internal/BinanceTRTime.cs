namespace Binance.TR.Api.Objects.Internal;

internal class BinanceTRTime
{
    [JsonProperty("timestamp"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime Time { get; set; }
}
