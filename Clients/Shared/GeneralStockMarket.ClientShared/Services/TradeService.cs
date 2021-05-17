using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

using GeneralStockMarket.ClientShared.Services.Interfaces;
using GeneralStockMarket.CoreLib.Response;
using GeneralStockMarket.DTO.Trade;

namespace GeneralStockMarket.ClientShared.Services
{
    public class TradeService : ITradeService
    {
        private readonly HttpClient httpClient;

        public TradeService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<Response<NoContent>> TradeAsync(TradeCreateDto tradeCreateDto)
        {
            var response = await httpClient.PostAsJsonAsync("api/trade",tradeCreateDto);
            return await response.Content.ReadFromJsonAsync<Response<NoContent>>();
        }
    }
}
