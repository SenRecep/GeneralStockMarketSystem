using System;

using GeneralStockMarket.CoreLib.Interfaces;

namespace GeneralStockMarket.DTO.MerketItem
{
    public class MarketItemUpdateDto:IDTO
    {
        public Guid Id { get; set; }
        public double Amount { get; set; }
        public bool InProgress { get; set; }
        public Guid UpdateUserId { get; set; }
    }
}
