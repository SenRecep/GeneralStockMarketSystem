using System;
using System.Collections.Generic;

using GeneralStockMarket.Core.Entities.Concrete;
using GeneralStockMarket.Entities.Interface;

namespace GeneralStockMarket.Entities.Concrete
{
    public class Wallet : EntityBase, IWallet
    {
        public Guid UserId { get; set; }
        public double Money { get; set; }
        public List<ProductItem> ProductItems { get; set; }
        public List<MarketItem> MarketItems { get; set; }
        public List<Transaction> TransactionSeller { get; set; }
        public List<Transaction> TransactionBuyer { get; set; }

    }
}
