namespace Binance.TR.Api.Enums;

/// <summary>
/// The type of an order
/// </summary>
public enum TimeInForce
{
    /// <summary>
    /// GoodTillCanceled orders will stay active until they are filled or canceled
    /// </summary>
    [Map("1", "GTC")]
    GoodTillCanceled = 1,

    /// <summary>
    /// ImmediateOrCancel orders have to be at least partially filled upon placing or will be automatically canceled
    /// </summary>
    [Map("2", "IOC")]
    ImmediateOrCancel = 2,

    /// <summary>
    /// FillOrKill orders have to be entirely filled upon placing or will be automatically canceled
    /// </summary>
    [Map("3", "FOK")]
    FillOrKill = 3,

    #region Not Supported by Binance TR
    /*
    /// <summary>
    /// GoodTillCrossing orders will post only
    /// </summary>
    [Map("4", "GTX")]
    GoodTillCrossing = 4,

    /// <summary>
    /// Good til the order expires or is canceled
    /// </summary>
    [Map("5", "GTE_GTC")]
    GoodTillExpiredOrCanceled = 5,

    /// <summary>
    /// Good til date
    /// </summary>
    [Map("6", "GTD")]
    GoodTillDate = 6
    */
    #endregion
}
