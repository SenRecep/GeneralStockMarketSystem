using System;

using GeneralStockMarket.Core.Entities.Abstract;
using GeneralStockMarket.Entities.Concrete;

namespace GeneralStockMarket.Entities.Interface
{
    public interface IItem : IEntityBase
    {
        public double Amount { get; set; }
        public Guid WalletId { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public Wallet Wallet { get; set; }
    }
}
