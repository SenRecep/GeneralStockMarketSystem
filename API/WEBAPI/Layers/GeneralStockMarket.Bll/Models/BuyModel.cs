using System;

using GeneralStockMarket.DTO.Wallet;
using GeneralStockMarket.Entities.Concrete;

namespace GeneralStockMarket.Bll.Models
{
    public class BuyModel
    {
        public Guid UserId { get; set; }
        public double Amount { get; set; }
        public Guid WalletId { get; set; }
        public Guid ProductId { get; set; }
        public ProductItem ProductItem { get; set; }
        public WalletDto WalletDto { get; set; }
    }
}