using System;

using GeneralStockMarket.CoreLib.Interfaces;
using GeneralStockMarket.DTO.Product;

namespace GeneralStockMarket.DTO.Request
{
    public class ProductDepositRequestDto:IDTO
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public ProductDto Product { get; set; }
        public string Description { get; set; }
        public bool Verify { get; set; }
        public double Amount { get; set; }
    }
}
