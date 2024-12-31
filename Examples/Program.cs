using Binance.TR.Api;
using Binance.TR.Api.Enums;
using System;
using System.Threading.Tasks;

namespace BinanceTR.Examples;

internal class Program
{
    static async Task Main(string[] args)
    {
        /*
        var api = new BinanceTRRestApiClient(new BinanceTRRestApiOptions
        {
            RawResponse = true,
        });

        var a01 = await api.GetTimeAsync();
        var a02 = await api.GetSymbolsAsync();
        var a03 = await api.GetOrderBookAsync("BTCTRY");
        var a04 = await api.GetTradesAsync("BTCTRY");
        var a05 = await api.GetAggregatedTradesAsync("BTCTRY");
        var a06 = await api.GetKlinesAsync("BTCTRY", KlineInterval.OneHour);
        var a07 = await api.PlaceOrderAsync("BTC_TRY", OrderSide.Buy, OrderType.Limit, 0.16m, price: 3_000_000);
        var a08 = await api.GetOrderAsync(10542);
        var a09 = await api.CancelOrderAsync(10542);
        var a10 = await api.GetOrdersAsync("BTC_TRY");
        var a11 = await api.PlaceOcoOrderAsync("BTC_TRY", OrderSide.Buy, 0.16m, 3_000_000m, 3_100_000m, 3_110_000m);
        var a12 = await api.GetAccountAsync();
        var a13 = await api.GetBalancesAsync();
        var a14 = await api.GetBalanceAsync("BTC");
        var a15 = await api.GetAccountTradesAsync("BTC_TRY");
        var a16 = await api.WithdrawAsync("BTC", 0.1m, "----ADDRESS-----");
        var a17 = await api.GetWithdrawalsAsync();
        var a18 = await api.GetDepositsAsync();
        var a19 = await api.GetDepositAddressAsync();
        var a20 = await api.CreateListenKeyAsync();
        var a21 = await api.ExtendListenKeyAsync(a20.Data);
        var a22 = await api.CloseListenKeyAsync(a20.Data);
        */

        var ws = new BinanceTRWebSocketApiClient(new BinanceTRWebSocketApiOptions
        {
            RawResponse = true,
        });

        /*
        await ws.SubscribeToTradesAsync("BTCUSDT", (data) =>
        {
            Console.WriteLine($"S:{data.Symbol} P:{data.Price} Q:{data.Quantity} ID:{data.TradeId} FID:{data.BuyerOrderId} LID:{data.SellerOrderId}");
        });

        await ws.SubscribeToAggregatedTradesAsync("BTCUSDT", (data) =>
        {
            Console.WriteLine($"S:{data.Symbol} P:{data.Price} Q:{data.Quantity} ID:{data.AggregateTradeId} FID:{data.FirstTradeId} LID:{data.LastTradeId}");
        });

        await ws.SubscribeToKlinesAsync("BTCUSDT", KlineInterval.FourHour, (data) =>
        {
            Console.WriteLine($"S:{data.Symbol} I:{data.Interval} P:{data.Open} Q:{data.High} ID:{data.Low} FID:{data.Close} LID:{data.BaseVolume}");
        });

        await ws.SubscribeToTickersAsync((data) =>
        {
            Console.WriteLine($"S:{data.Symbol} L:{data.LastPrice} O:{data.OpenPrice} H:{data.HighPrice} L:{data.LowPrice} BV:{data.BaseVolume} QV:{data.QuoteVolume}");
        });

        await ws.SubscribeToPartialDepthAsync("BTCUSDT", 20, 1000, (data) =>
        {
            Console.WriteLine($"A:{data.Asks.Count} B:{data.Bids.Count} U:{data.LastUpdateId}");
        });
        */

        await ws.SubscribeToDepthDiffAsync("BTCTRY",  100, (data) =>
        {
            Console.WriteLine($"A:{data.Asks.Count} B:{data.Bids.Count} U:{data.LastUpdateId}");
        });

        
        Console.WriteLine("SUBSCRIBED!..");
        Console.ReadLine();








        var a = 0;

        /*
        var ws = new BinanceTRSocketClient(new BinanceTRWebSocketApiClientOptions
        {
            LogLevel = Microsoft.Extensions.Logging.LogLevel.Debug,
            ApiCredentials = new CryptoExchange.Net.Authentication.ApiCredentials("81fA46302354762b4874E7F0a69dD05CAu9ENUGRZwRe2vgjaUjYMvwvVG6Gaif1", "da55362C6f0634ea18E430c199bD8cc7uScAMAy9L0SfNhTinOOBaUwQASuyl6ME")
        });

        /*
        ws.SubscribeToAggregatedTradeUpdates("BTCTRY", (data) =>
        {
            if (data != null)
            {
                Console.WriteLine($"S:{data.Data.Symbol} P:{data.Data.Price} Q:{data.Data.Quantity} ID:{data.Data.AggregateTradeId} FID:{data.Data.FirstTradeId} LID:{data.Data.LastTradeId}");
            }
        });

        ws.SubscribeToTradeUpdates("BTCTRY", (data) =>
        {
            if (data != null)
            {
                Console.WriteLine($"S:{data.Data.Symbol} P:{data.Data.Price} Q:{data.Data.Quantity}");
            }
        });

        var ob = new BinanceTROrderBookClient("BTCTRY", new BinanceTROrderBookClientOptions
        {
            RestClient = cli,
            SocketClient = ws,
        });
        var res = ob.StartAsync().Result;
        Console.WriteLine("Başladı");
        Console.ReadLine();

        while (true)
        {
            Console.WriteLine($"S:{ob.Symbol} BP:{ob.BestBid.Price} BQ:{ob.BestBid.Quantity} AP:{ob.BestAsk.Price} AQ:{ob.BestAsk.Quantity}");
            Thread.Sleep(1000);
        }

        Console.WriteLine("Done");
        Console.ReadLine();
        */
    }
}
