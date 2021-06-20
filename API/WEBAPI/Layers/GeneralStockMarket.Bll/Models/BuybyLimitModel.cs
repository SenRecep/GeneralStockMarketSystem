using System;

using GeneralStockMarket.DTO.Wallet;
using GeneralStockMarket.Entities.Concrete;

namespace GeneralStockMarket.Bll.Models
{
    public class BuybyLimitModel
    {
        public Guid UserId { get; set; }
        public double Amount { get; set; }
        public double Money { get; set; }
        public double UnitPrice { get; set; }
        public Guid WalletId { get; set; }
        public Guid ProductId { get; set; }
    }
}
