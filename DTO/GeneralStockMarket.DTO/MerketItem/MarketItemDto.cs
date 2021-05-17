using System;

using GeneralStockMarket.CoreLib.Interfaces;

namespace GeneralStockMarket.DTO.MerketItem
{
    public class MarketItemDto : IDTO
    {
        public Guid Id { get; set; }
        public double Amount { get; set; }
        public double UnitPrice { get; set; }
        public bool InProgress { get; set; }
        public Guid WalletId { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
