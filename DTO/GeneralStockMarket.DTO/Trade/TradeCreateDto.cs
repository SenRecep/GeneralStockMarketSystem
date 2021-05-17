
using System;

using GeneralStockMarket.CoreLib.Interfaces;
using GeneralStockMarket.DTO.General;

namespace GeneralStockMarket.DTO.Trade
{
    public class TradeCreateDto : IDTO
    {
        public Guid ProductId { get; set; }
        public double Price { get; set; }
        public double Amount { get; set; }
        public TradeType TradeType { get; set; } 
    }
}
