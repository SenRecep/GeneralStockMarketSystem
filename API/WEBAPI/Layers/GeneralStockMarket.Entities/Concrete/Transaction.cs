using System;

using GeneralStockMarket.Core.Entities.Concrete;
using GeneralStockMarket.Entities.Interface;

namespace GeneralStockMarket.Entities.Concrete
{
    public class Transaction : EntityBase, ITransaction
    {
        public Guid WalletIdSeller { get; set; }
        public Guid WalletIdBuyer { get; set; }
        public double UnitPrice { get; set; }
        public double Amount { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public Wallet WalletBuyer { get; set; }
        public Wallet WalletSeller { get; set; }
    }
}
