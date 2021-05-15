using System;

using GeneralStockMarket.DTO.Product;

namespace GeneralStockMarket.DTO.ProductItem
{
    public class ProductItemDto
    {
        public Guid Id { get; set; }
        public double Amount { get; set; }
        public Guid ProductId { get; set; }
        public ProductDto Product { get; set; }
    }
}
