﻿namespace Binance.TR.Api.Enums;

/// <summary>
/// The type of an order
/// </summary>
public enum OrderType
{
    /// <summary>
    /// Limit orders will be placed at a specific price. If the price isn't available in the order book for that asset the order will be added in the order book for someone to fill.
    /// </summary>
    [Map("1")]
    Limit = 1,

    /// <summary>
    /// Market order will be placed without a price. The order will be executed at the best price available at that time in the order book.
    /// </summary>
    [Map("2")]
    Market = 2,

    /*
    /// <summary>
    /// Stop loss order.
    /// </summary>
    [Map("3")]
    StopLoss = 3,
    */

    /// <summary>
    /// Stop loss order. Will execute a limit order when the price drops below a price to sell and therefor limit the loss
    /// </summary>
    [Map("4")]
    StopLossLimit = 4,

    /*
    /// <summary>
    /// Take profit order.
    /// </summary>
    [Map("5")]
    TakeProfit = 5,
    */

    /// <summary>
    /// Take profit order. Will execute a limit order when the price rises above a price to sell and therefor take a profit
    /// </summary>
    [Map("6")]
    TakeProfitLimit = 6,

    /*
    /// <summary>
    /// Same as a limit order, however it will fail if the order would immediately match, therefor preventing taker orders
    /// </summary>
    [Map("7")]
    LimitMaker = 7,
    */
}
