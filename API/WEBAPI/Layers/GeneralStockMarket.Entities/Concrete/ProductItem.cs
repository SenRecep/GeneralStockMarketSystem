using System;

using GeneralStockMarket.Core.Entities.Concrete;
using GeneralStockMarket.Entities.Interface;

namespace GeneralStockMarket.Entities.Concrete
{
    public class ProductItem : EntityBase, IProductItem
    {
        public double Amount { get; set; }
        public Guid WalletId { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public Wallet Wallet { get; set; }
    }
}
