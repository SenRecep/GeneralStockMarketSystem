using System;

using GeneralStockMarket.CoreLib.Interfaces;

namespace GeneralStockMarket.DTO.Request
{
    public class NewTypeRequestDto : IDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Verify { get; set; }
    }
}
