
using System;

using GeneralStockMarket.CoreLib.Interfaces;
using GeneralStockMarket.DTO.Request.Enums;

namespace GeneralStockMarket.DTO.Request
{
    public class CreateDepositRequest : IDTO
    {
        public string Description { get; set; }
        public double Amount { get; set; }
        public Guid UserId { get; set; }
        public MoneyType MoneyType { get; set; }

    }
}
