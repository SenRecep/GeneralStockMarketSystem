using System;
using System.Collections.Generic;

using GeneralStockMarket.Core.Entities.Abstract;
using GeneralStockMarket.Entities.Concrete;

namespace GeneralStockMarket.Entities.Interface
{
    public interface IWallet : IEntityBase,IUserDependent
    {
        public double Money { get; set; }
        public List<ProductItem> ProductItems { get; set; }
        public List<MarketItem> MarketItems { get; set; }
        public List<Transaction> TransactionSeller { get; set; }
        public List<Transaction> TransactionBuyer { get; set; }
    }
}
