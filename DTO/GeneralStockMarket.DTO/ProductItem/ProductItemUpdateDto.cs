using System;

using GeneralStockMarket.CoreLib.Interfaces;

namespace GeneralStockMarket.DTO.ProductItem
{
    public class ProductItemUpdateDto : IDTO
    {
        public Guid Id { get; set; }
        public double Amount { get; set; }
        public Guid UpdateUserId { get; set; }
    }
}
