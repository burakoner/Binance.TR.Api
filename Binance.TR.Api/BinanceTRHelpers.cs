namespace Binance.TR.Api;

/// <summary>
/// Helper methods for the Binance API
/// </summary>
public static class BinanceTRHelpers
{
    /// <summary>
    /// Clamp a quantity between a min and max quantity and floor to the closest step
    /// </summary>
    /// <param name="minQuantity"></param>
    /// <param name="maxQuantity"></param>
    /// <param name="stepSize"></param>
    /// <param name="quantity"></param>
    /// <returns></returns>
    public static decimal ClampQuantity(decimal minQuantity, decimal maxQuantity, decimal stepSize, decimal quantity)
    {
        quantity = Math.Min(maxQuantity, quantity);
        quantity = Math.Max(minQuantity, quantity);
        if (stepSize == 0)
            return quantity;
        quantity -= quantity % stepSize;
        quantity = Floor(quantity);
        return quantity;
    }

    /// <summary>
    /// Clamp a price between a min and max price
    /// </summary>
    /// <param name="minPrice"></param>
    /// <param name="maxPrice"></param>
    /// <param name="price"></param>
    /// <returns></returns>
    public static decimal ClampPrice(decimal minPrice, decimal maxPrice, decimal price)
    {
        price = Math.Min(maxPrice, price);
        price = Math.Max(minPrice, price);
        return price;
    }

    /// <summary>
    /// Floor a price to the closest tick
    /// </summary>
    /// <param name="tickSize"></param>
    /// <param name="price"></param>
    /// <returns></returns>
    public static decimal FloorPrice(decimal tickSize, decimal price)
    {
        price -= price % tickSize;
        price = Floor(price);
        return price;
    }

    /// <summary>
    /// Floor
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    public static decimal Floor(decimal number)
    {
        return Math.Floor(number * 100000000) / 100000000;
    }

#if NETSTANDARD2_0
    /// <summary>
    /// Contains method for netstandard2.0.
    /// Returns a value indicating whether a specified string occurs within this string, using the specified comparison rules.
    /// </summary>
    /// <param name="this">Source string</param>
    /// <param name="value">The string to seek.</param>
    /// <param name="comparisonType">One of the enumeration values that specifies the rules to use in the comparison.</param>
    /// <returns>true if the value parameter occurs within this string, or if value is the empty string (""); otherwise, false.</returns>
    public static bool Contains(this string @this, string value, StringComparison comparisonType)
    {
        return @this.IndexOf(value, comparisonType) >= 0;
    }
#endif
}