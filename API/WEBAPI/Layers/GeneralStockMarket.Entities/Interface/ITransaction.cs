using System;

using GeneralStockMarket.Core.Entities.Abstract;
using GeneralStockMarket.Entities.Concrete;

namespace GeneralStockMarket.Entities.Interface
{
    public interface ITransaction : IEntityBase
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
