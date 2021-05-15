using System;

using GeneralStockMarket.CoreLib.Interfaces;

namespace GeneralStockMarket.DTO.Request
{
    public class DepositRequestDto : IDTO
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public bool Verify { get; set; }
        public double Amount { get; set; }
    }
}
