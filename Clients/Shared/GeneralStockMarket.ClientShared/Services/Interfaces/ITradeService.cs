using System.Threading.Tasks;

using GeneralStockMarket.CoreLib.Response;
using GeneralStockMarket.DTO.Trade;

namespace GeneralStockMarket.ClientShared.Services.Interfaces
{
    public interface ITradeService
    {
        public Task<Response<NoContent>> TradeAsync(TradeCreateDto tradeCreateDto);
    }
}
