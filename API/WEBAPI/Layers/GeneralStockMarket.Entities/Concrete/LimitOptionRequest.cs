using System;

using GeneralStockMarket.Core.Entities.Concrete;
using GeneralStockMarket.Entities.Interface;

namespace GeneralStockMarket.Entities.Concrete
{
    public class LimitOptionRequest : EntityBase, ILimitOptionRequest
    {
        public Guid WalletId { get; set; }
        public Guid ProductId { get; set; }
        public double Amount { get; set; }
        public double Money { get; set; }
        public double UnitPrice { get; set; }
        public bool InProgress { get; set; }

        public Wallet Wallet { get; set; }
        public Product Product { get; set; }
    }
}
