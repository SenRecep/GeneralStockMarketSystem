
using System;

using GeneralStockMarket.CoreLib.Interfaces;

using Microsoft.AspNetCore.Http;

namespace GeneralStockMarket.DTO.Product
{
    public class ProductCreateDto : IDTO
    {
        public string Name { get; set; }
        public string ImageName { get; set; }
        public IFormFile Image { get; set; }
        public Guid CreatedUserId { get; set; }
    }
}
