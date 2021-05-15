using System;

using GeneralStockMarket.CoreLib.Interfaces;

namespace GeneralStockMarket.DTO.Product
{
    public class ProductUpdateDto : IDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid UpdateUserId { get; set; }
    }
}
