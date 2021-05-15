using System;

using GeneralStockMarket.Core.Entities.Concrete;
using GeneralStockMarket.Entities.Interface;

namespace GeneralStockMarket.Entities.Concrete
{
    public class MarketItem : EntityBase, IMarketItem
    {
        public double Amount { get; set; }
        public Guid WalletId { get; set; }
        public Guid ProductId { get; set; }
        public double UnitPrice { get; set; }
        public bool InProgress { get; set; }
        public Product Product { get; set; }
        public Wallet Wallet { get; set; }
    }
}
