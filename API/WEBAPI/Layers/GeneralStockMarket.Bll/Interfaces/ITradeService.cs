
using System.Threading.Tasks;

using GeneralStockMarket.Bll.Models;
using GeneralStockMarket.CoreLib.Response;
using GeneralStockMarket.Entities.Concrete;

namespace GeneralStockMarket.Bll.Interfaces
{
    public interface ITradeService
    {
        public Task<Response<NoContent>> SellAsync(SellModel buyModel);
        public Task<Response<NoContent>> BuyAsync(BuyModel sellModel);
        public (ProductItem ProductItem, MarketItem MarketItem) CreateMarketItem(SellModel sellModel);
    }
}
