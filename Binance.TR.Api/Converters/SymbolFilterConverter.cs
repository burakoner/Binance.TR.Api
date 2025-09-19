namespace Binance.TR.Api.Converters;

internal class SymbolFilterConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return false;
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
#pragma warning disable 8604, 8602
        var obj = JObject.Load(reader);
        var type = new SymbolFilterTypeConverter(false).ReadString(obj["filterType"].ToString());
        var applyToMarket = (bool)obj["applyToMarket"];
        BinanceTRSymbolFilter result;
        switch (type)
        {
            case SymbolFilterType.Price:
                result = new BinanceSymbolPriceFilter
                {
                    MaxPrice = (decimal)obj["maxPrice"],
                    MinPrice = (decimal)obj["minPrice"],
                    TickSize = (decimal)obj["tickSize"],
                    ApplyToMarket = (bool)obj["applyToMarket"]
                };
                break;

            case SymbolFilterType.LotSize:
                result = new BinanceSymbolLotSizeFilter
                {
                    MaxQuantity = (decimal)obj["maxQty"],
                    MinQuantity = (decimal)obj["minQty"],
                    StepSize = (decimal)obj["stepSize"],
                    ApplyToMarket = (bool)obj["applyToMarket"]
                };
                break;

            case SymbolFilterType.IcebergParts:
                result = new BinanceSymbolIcebergPartsFilter
                {
                    Limit = (int)obj["limit"],
                    ApplyToMarket = (bool)obj["applyToMarket"]
                };
                break;

            case SymbolFilterType.MarketLotSize:
                result = new BinanceSymbolMarketLotSizeFilter
                {
                    MaxQuantity = (decimal)obj["maxQty"],
                    MinQuantity = (decimal)obj["minQty"],
                    StepSize = (decimal)obj["stepSize"],
                    ApplyToMarket = (bool)obj["applyToMarket"]
                };
                break;

            case SymbolFilterType.TrailingDelta:
                result = new BinanceSymbolTrailingDeltaFilter
                {
                    ApplyToMarket = (bool)obj["applyToMarket"]
                };
                break;

            case SymbolFilterType.PercentPriceBySide:
                result = new BinanceSymbolPercentPriceBySideFilter
                {
                    BidMultiplierUp = (decimal)obj["bidMultiplierUp"],
                    BidMultiplierDown = (decimal)obj["bidMultiplierDown"],
                    AskMultiplierUp = (decimal)obj["askMultiplierUp"],
                    AskMultiplierDown = (decimal)obj["askMultiplierDown"],
                    AveragePriceMinutes = (int)obj["avgPriceMins"],
                    ApplyToMarket = (bool)obj["applyToMarket"]
                };
                break;

            case SymbolFilterType.Notional:
                result = new BinanceSymbolNotionalFilter
                {
                    MinNotional = (decimal)obj["minNotional"],
                    AveragePriceMinutes = (int)obj["avgPriceMins"],
                    ApplyToMarket = (bool)obj["applyToMarket"]
                };
                break;

            case SymbolFilterType.MaxNumberOfOrders:
                result = new BinanceSymbolMaxOrdersFilter
                {
                    ApplyToMarket = (bool)obj["applyToMarket"]
                };
                break;
                
            case SymbolFilterType.MaxNumberOfAlgorithmicOrders:
                result = new BinanceSymbolMaxAlgorithmicOrdersFilter
                {
                    MaxNumberAlgorithmicOrders = (int)obj["maxNumAlgoOrders"],
                    ApplyToMarket = (bool)obj["applyToMarket"]
                };
                break;

            case SymbolFilterType.MaxNumberOfOrderLists:
                result = new BinanceSymbolMaxOrderListsFilter
                {
                    ApplyToMarket = (bool)obj["applyToMarket"]
                };
                break;

            case SymbolFilterType.MaxNumberOfOrderAmends:
                result = new BinanceSymbolMaxOrderAmendsFilter
                {
                    ApplyToMarket = (bool)obj["applyToMarket"]
                };
                break;

            default:
                Debug.WriteLine("Can't parse symbol filter of type: " + obj["filterType"]);
                result = new BinanceTRSymbolFilter();
                break;
        }
#pragma warning restore 8604
        result.ApplyToMarket = applyToMarket;
        result.FilterType = type;
        return result;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        var filter = (BinanceTRSymbolFilter)value!;
        writer.WriteStartObject();

        writer.WritePropertyName("filterType");
        writer.WriteValue(JsonConvert.SerializeObject(filter.FilterType, new SymbolFilterTypeConverter(false)));

        writer.WritePropertyName("applyToMarket");
        writer.WriteValue(filter.ApplyToMarket);

        switch (filter.FilterType)
        {
            case SymbolFilterType.Price:
                var priceFilter = (BinanceSymbolPriceFilter)filter;
                writer.WritePropertyName("maxPrice");
                writer.WriteValue(priceFilter.MaxPrice);
                writer.WritePropertyName("minPrice");
                writer.WriteValue(priceFilter.MinPrice);
                writer.WritePropertyName("tickSize");
                writer.WriteValue(priceFilter.TickSize);
                break;
            case SymbolFilterType.LotSize:
                var lotSizeFilter = (BinanceSymbolLotSizeFilter)filter;
                writer.WritePropertyName("maxQty");
                writer.WriteValue(lotSizeFilter.MaxQuantity);
                writer.WritePropertyName("minQty");
                writer.WriteValue(lotSizeFilter.MinQuantity);
                writer.WritePropertyName("stepSize");
                writer.WriteValue(lotSizeFilter.StepSize);
                break;
            case SymbolFilterType.IcebergParts:
                var icebergPartsFilter = (BinanceSymbolIcebergPartsFilter)filter;
                writer.WritePropertyName("limit");
                writer.WriteValue(icebergPartsFilter.Limit);
                break;
            case SymbolFilterType.MarketLotSize:
                var marketLotSizeFilter = (BinanceSymbolMarketLotSizeFilter)filter;
                writer.WritePropertyName("maxQty");
                writer.WriteValue(marketLotSizeFilter.MaxQuantity);
                writer.WritePropertyName("minQty");
                writer.WriteValue(marketLotSizeFilter.MinQuantity);
                writer.WritePropertyName("stepSize");
                writer.WriteValue(marketLotSizeFilter.StepSize);
                break;
            case SymbolFilterType.TrailingDelta:
                var trailingDeltaFilter = (BinanceSymbolTrailingDeltaFilter)filter;
                break;
            case SymbolFilterType.PercentPriceBySide:
                var pricePercentFilter = (BinanceSymbolPercentPriceBySideFilter)filter;
                writer.WritePropertyName("bidMultiplierUp");
                writer.WriteValue(pricePercentFilter.BidMultiplierUp);
                writer.WritePropertyName("bidMultiplierDown");
                writer.WriteValue(pricePercentFilter.BidMultiplierDown);
                writer.WritePropertyName("askMultiplierUp");
                writer.WriteValue(pricePercentFilter.AskMultiplierUp);
                writer.WritePropertyName("askMultiplierDown");
                writer.WriteValue(pricePercentFilter.AskMultiplierDown);
                writer.WritePropertyName("avgPriceMins");
                writer.WriteValue(pricePercentFilter.AveragePriceMinutes);
                break;
            case SymbolFilterType.Notional:
                var minNotionalFilter = (BinanceSymbolNotionalFilter)filter;
                writer.WritePropertyName("minNotional");
                writer.WriteValue(minNotionalFilter.MinNotional);
                writer.WritePropertyName("avgPriceMins");
                writer.WriteValue(minNotionalFilter.AveragePriceMinutes);
                break;
            case SymbolFilterType.MaxNumberOfOrders:
                var maxOrdersFilter = (BinanceSymbolMaxOrdersFilter)filter;
                break;
            case SymbolFilterType.MaxNumberOfAlgorithmicOrders:
                var maxAlgorithmicOrdersFilter = (BinanceSymbolMaxAlgorithmicOrdersFilter)filter;
                writer.WritePropertyName("maxNumAlgoOrders");
                writer.WriteValue(maxAlgorithmicOrdersFilter.MaxNumberAlgorithmicOrders);
                break;

            case SymbolFilterType.MaxNumberOfOrderLists:
                var maxOrderListsFilter = (BinanceSymbolMaxOrderListsFilter)filter;
                break;
            case SymbolFilterType.MaxNumberOfOrderAmends:
                var maxOrderAmendsFilter = (BinanceSymbolMaxOrderAmendsFilter)filter;
                break;
            default:
                Debug.WriteLine("Can't write symbol filter of type: " + filter.FilterType);
                break;
        }

        writer.WriteEndObject();
    }
}
