
using System;

using GeneralStockMarket.CoreLib.Interfaces;

namespace GeneralStockMarket.DTO.Product
{
    public class ProductDto : IDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ImageName { get; set; }
        public double Amount { get; set; }
        public double AvgPrice { get; set; }
    }
}
