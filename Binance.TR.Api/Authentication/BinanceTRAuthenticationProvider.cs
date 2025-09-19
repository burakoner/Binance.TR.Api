namespace Binance.TR.Api.Authentication;

internal class BinanceTRAuthenticationProvider : AuthenticationProvider
{
    public BinanceTRAuthenticationProvider(ApiCredentials credentials) : base(credentials ?? new ApiCredentials("", ""))
    {
        //if (credentials is null || credentials.Secret is null)
        //    throw new ArgumentException("No valid API credentials provided. Key, Secret and PassPhrase needed.");
    }

    public override void AuthenticateRestApi(RestApiClient apiClient, Uri uri, HttpMethod method, bool signed, ArraySerialization serialization, SortedDictionary<string, object> query, SortedDictionary<string, object> body, string bodyContent, SortedDictionary<string, string> headers)
    {
        // Check Point
        if (!signed)
            return;

        // Check Point
        if (Credentials is null || Credentials.Key is null || Credentials.Secret is null || string.IsNullOrEmpty(Credentials.Key.GetString()))
            throw new ArgumentException("No valid API credentials provided. Key/Secret/PassPhrase needed.");

        // Timestamp
        var timestamp = GetMillisecondTimestamp(apiClient);

        // Fix Parameters
        body ??= [];
        query ??= [];
        headers ??= [];

        // Parameter Position
        var options = (BinanceTRRestApiOptions)apiClient.ClientOptions;
        var paramsInBody = (method == HttpMethod.Post || method == HttpMethod.Put || method == HttpMethod.Delete);
        if (paramsInBody && uri.AbsoluteUri.Contains("?")) paramsInBody = false;
        if (paramsInBody)
        {
            body.Add("timestamp", timestamp);
            body.Add("recvWindow", options.ReceiveWindow.TotalMilliseconds);
        }
        else
        {
            query.Add("timestamp", timestamp);
            query.Add("recvWindow", options.ReceiveWindow.TotalMilliseconds);
        }

        // Signature
        var totalParams = new Dictionary<string, object>();
        if (body.Count > 0) foreach (var param in body) totalParams.Add(param.Key, param.Value);
        if (query.Count > 0) foreach (var param in query) totalParams.Add(param.Key, param.Value);
        var signature = SignHMACSHA256(totalParams.ToFormData(), SignatureOutputType.Hex).ToLower();

        // Headers
        headers.Add("X-MBX-APIKEY", Credentials.Key.GetString());

        // Add Signature
        if (paramsInBody) body.Add("signature", signature);
        else query.Add("signature", signature);
    }

    public static string Base64Encode(byte[] plainBytes) => Convert.ToBase64String(plainBytes);
    public static string Base64Encode(string plainText) => Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));
    public static string Base64Decode(string base64EncodedData) => Encoding.UTF8.GetString(Convert.FromBase64String(base64EncodedData));
}
