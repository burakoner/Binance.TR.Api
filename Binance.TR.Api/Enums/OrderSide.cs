namespace Binance.TR.Api.Enums;

/// <summary>
/// The side of an order
/// </summary>
public enum OrderSide
{
    /// <summary>
    /// Buy
    /// </summary>
    [Map("0", "BUY")]
    Buy = 0,

    /// <summary>
    /// Sell
    /// </summary>
    [Map("1", "SELL")]
    Sell = 1,
}
