using System;

using GeneralStockMarket.CoreLib.Interfaces;
using GeneralStockMarket.DTO.Request.Enums;

namespace GeneralStockMarket.DTO.Request
{
    public class DepositRequestDto : IDTO
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public bool? Verify { get; set; }
        public double Amount { get; set; }
        public Guid CreatedUserId { get; set; }
        public DateTime CreatedTime { get; set; }
        public MoneyType MoneyType { get; set; }
    }
}
