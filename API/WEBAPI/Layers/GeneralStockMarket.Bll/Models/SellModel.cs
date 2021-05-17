using System;

namespace GeneralStockMarket.Bll.Models
{
    public class SellModel
    {
        public Guid UserId { get; set; }
        public double Amount { get; set; }
        public Guid WalletId { get; set; }
        public Guid ProductId { get; set; }
        public double UnitPrice { get; set; }
    }
}
