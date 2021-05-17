
using System.Threading.Tasks;

using GeneralStockMarket.Bll.Interfaces;
using GeneralStockMarket.Bll.Models;
using GeneralStockMarket.CoreLib.ExtensionMethods;
using GeneralStockMarket.CoreLib.Response;
using GeneralStockMarket.Dal.Interface;
using GeneralStockMarket.Entities.Concrete;

using Microsoft.AspNetCore.Http;

namespace GeneralStockMarket.Bll.Managers
{
    public class TradeManager : ITradeService
    {
        private readonly ITradeRepository tradeRepository;
        private readonly IGenericRepository<ProductItem> genericProductItemRepository;
        private readonly IGenericRepository<MarketItem> genericMarketItemRepository;

        public TradeManager(
            ITradeRepository tradeRepository,
         IGenericRepository<ProductItem> genericProductItemRepository,
        IGenericRepository<MarketItem> genericMarketItemRepository)
        {
            this.tradeRepository = tradeRepository;
            this.genericProductItemRepository = genericProductItemRepository;
            this.genericMarketItemRepository = genericMarketItemRepository;
        }
        public (ProductItem ProductItem, MarketItem MarketItem) CreateMarketItem(SellModel sellModel)
        {
            if (sellModel.Amount > sellModel.ProductItem.Amount)
                return new(sellModel.ProductItem, null);

            MarketItem marketItem = new()
            {
                Amount = sellModel.Amount,
                CreatedUserId = sellModel.UserId,
                InProgress = true,
                ProductId = sellModel.ProductId,
                UnitPrice = sellModel.UnitPrice,
                WalletId = sellModel.WalletId
            };
            sellModel.ProductItem.Amount -= sellModel.Amount;
            return new(sellModel.ProductItem, marketItem);
        }

        public async Task<Response<NoContent>> SellAsync(SellModel sellModel)
        {
            var sell = CreateMarketItem(sellModel);
            if (sell.MarketItem.IsNull())
            {
                var response = Response<NoContent>.Fail(
                    statusCode: StatusCodes.Status400BadRequest,
                    isShow: true,
                    path: "[Post] api/trade",
                    errors: "Satma islemi iptal edildi, yeteri kadarda ürününüz mevcut değil"
                    );
                return response;
            }
            try
            {
                await genericMarketItemRepository.AddAsync(sell.MarketItem);
                await genericMarketItemRepository.Commit();
                await genericProductItemRepository.UpdateAsync(sell.ProductItem);
                await genericProductItemRepository.Commit();
                var response = Response<NoContent>.Success(StatusCodes.Status201Created);
                return response;
            }
            catch
            {
                await genericMarketItemRepository.Commit(false);
                await genericProductItemRepository.Commit(false);
                var response = Response<NoContent>.Fail(
                   statusCode: StatusCodes.Status500InternalServerError,
                   isShow: true,
                   path: "[Post] api/trade",
                   errors: "Satma islemi gerçekleşirken bir hata ile karşılaşıldı ve işlem iptal edildi"
                   );
                return response;
            }
        }
    }
}
