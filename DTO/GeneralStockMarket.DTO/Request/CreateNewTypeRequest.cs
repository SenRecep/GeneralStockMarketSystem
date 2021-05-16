
using System;

using GeneralStockMarket.CoreLib.Interfaces;

namespace GeneralStockMarket.DTO.Request
{
    public class CreateNewTypeRequest : IDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid UserId { get; set; }


    }
}
