namespace Binance.TR.Api;

/// <summary>
/// OKX Rest API Options
/// </summary>
public class BinanceTRRestApiOptions : RestApiClientOptions
{
    /// <summary>
    /// Receive Window
    /// </summary>
    public TimeSpan ReceiveWindow { get; set; }

    /// <summary>
    /// Auto Timestamp
    /// </summary>
    public bool AutoTimestamp { get; set; }

    /// <summary>
    /// Auto Timestamp Interval
    /// </summary>
    public TimeSpan AutoTimestampInterval { get; set; }

    /// <summary>
    /// Constructor
    /// </summary>
    public BinanceTRRestApiOptions() : this(null)
    {
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="credentials">OkxApiCredentials</param>
    public BinanceTRRestApiOptions(ApiCredentials credentials)
    {
        // API Credentials
        ApiCredentials = credentials;

        // Receive Window
        ReceiveWindow = TimeSpan.FromSeconds(5);

        // Auto Timestamp
        AutoTimestamp = true;
        AutoTimestampInterval = TimeSpan.FromHours(1);

        // Http Options
        HttpOptions = new HttpOptions
        {
            UserAgent = RestApiConstants.USER_AGENT,
            AcceptMimeType = RestApiConstants.JSON_CONTENT_HEADER,
            RequestTimeout = TimeSpan.FromSeconds(30),
            EncodeQueryString = true,
        };
    }
}
