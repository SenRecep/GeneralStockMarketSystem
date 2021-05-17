using System;

using GeneralStockMarket.DTO.ProductItem;
using GeneralStockMarket.Entities.Concrete;

namespace GeneralStockMarket.Bll.Models
{
    public class SellModel
    {
        public Guid UserId { get; set; }
        public double Amount { get; set; }
        public Guid WalletId { get; set; }
        public Guid ProductId { get; set; }
        public double UnitPrice { get; set; }
        public ProductItem ProductItem { get; set; }
    }
}
