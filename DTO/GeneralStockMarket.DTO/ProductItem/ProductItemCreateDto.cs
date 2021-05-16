using System;

using GeneralStockMarket.CoreLib.Interfaces;

namespace GeneralStockMarket.DTO.ProductItem
{
    public class ProductItemCreateDto:IDTO
    {
        public double Amount { get; set; }
        public Guid WalletId { get; set; }
        public Guid ProductId { get; set; }
        public Guid CreatedUserId { get; set; }
    }
}
