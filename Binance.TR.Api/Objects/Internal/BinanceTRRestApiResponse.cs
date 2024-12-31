namespace Binance.TR.Api.Objects.Internal;

internal class BinanceTRRestApiResponse<T>
{
    [JsonProperty("code")]
    public int Code { get; set; }

    [JsonProperty("msg")]
    public string Message { get; set; }

    [JsonProperty("timestamp")]
    public long Timestamp { get; set; }

    [JsonIgnore]
    public DateTime Time { get => Timestamp.ConvertFromMilliseconds(); }

    [JsonProperty("data")]
    public T Payload { get; set; }
}

internal class BinanceTRRestApiListResponse<T>
{
    [JsonProperty("list")]
    public List<T> List { get; set; }
}