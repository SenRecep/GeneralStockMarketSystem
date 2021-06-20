using System.Collections.Generic;

using GeneralStockMarket.Core.Entities.Concrete;
using GeneralStockMarket.Entities.Interface;

namespace GeneralStockMarket.Entities.Concrete
{
    public class Product : EntityBase, IProduct
    {
        public string Name { get; set; }
        public string ImageName { get; set; }
        public List<ProductItem> ProductItems { get; set; }
        public List<MarketItem> MarketItems { get; set; }
        public List<Transaction> Transactions { get; set; }
        public List<LimitOptionRequest> LimitOptionRequests { get; set; }

    }
}
