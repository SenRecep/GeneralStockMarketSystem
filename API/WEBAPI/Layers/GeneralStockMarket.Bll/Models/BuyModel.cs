using System;
using GeneralStockMarket.Entities.Concrete;

namespace GeneralStockMarket.Bll.Models
{
    public class BuyModel
    {
        public Guid UserId { get; set; }
        public double Amount { get; set; }
        public Guid BuyerWalletId { get; set; }
        public Guid SellerWalletId { get; set; }
        public Guid ProductId { get; set; }
        public ProductItem ProductItem { get; set; }
    }
}