using System;

using GeneralStockMarket.CoreLib.Interfaces;

namespace GeneralStockMarket.DTO.Request
{
    public class VerifyDto:IDTO
    {
        public Guid Id { get; set; }
        public Guid UpdateUserId { get; set; }
        public bool? Verify { get; set; }
    }
}
