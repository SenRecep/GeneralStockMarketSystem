using System;

using GeneralStockMarket.CoreLib.Interfaces;

namespace GeneralStockMarket.DTO.General
{
    public class DeleteDto:IDTO
    {
        public Guid Id { get; set; }
        public Guid UpdateUserId { get; set; }
    }
}
