
using System.Threading.Tasks;

using GeneralStockMarket.Bll.Models;
using GeneralStockMarket.CoreLib.Response;
using GeneralStockMarket.Entities.Concrete;

namespace GeneralStockMarket.Bll.Interfaces
{
    public interface ITradeService
    {
        public Task<Response<NoContent>> SellAsync(SellModel sellModel);
        public Task<Response<NoContent>> BuyAsync(BuyModel buyModel);
        public Task<Response<NoContent>> BuybyLimitAsync(BuybyLimitModel buyModel);
        public (ProductItem ProductItem, MarketItem MarketItem) CreateMarketItem(SellModel sellModel);
    }
}
