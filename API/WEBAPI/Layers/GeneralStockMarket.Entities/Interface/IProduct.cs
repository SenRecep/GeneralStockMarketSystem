using System.Collections.Generic;

using GeneralStockMarket.Core.Entities.Abstract;
using GeneralStockMarket.Entities.Concrete;

namespace GeneralStockMarket.Entities.Interface
{
    public interface IProduct : IEntityBase
    {
        public string Name { get; set; }
        public string ImageName { get; set; }
        public List<ProductItem> ProductItems { get; set; }
        public List<MarketItem> MarketItems { get; set; }
        public List<Transaction> Transactions { get; set; }
        public List<LimitOptionRequest> LimitOptionRequests { get; set; }
    }
}
