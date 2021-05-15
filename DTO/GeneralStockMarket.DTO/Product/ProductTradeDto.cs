using System;
using System.Collections.Generic;

using GeneralStockMarket.CoreLib.Interfaces;
using GeneralStockMarket.DTO.MerketItem;

namespace GeneralStockMarket.DTO.Product
{
    public class ProductTradeDto : IDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ImageName { get; set; }
        public double Amount { get; set; }
        public double AvgPrice { get; set; }
        public IEnumerable<MarketItemDto> MarketItems { get; set; }
    }
}
