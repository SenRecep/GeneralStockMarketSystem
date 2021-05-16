using System;

using GeneralStockMarket.CoreLib.Interfaces;

namespace GeneralStockMarket.DTO.Request
{
    public class CreateProductDepositRequest : IDTO
    {
        public Guid ProductId { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public Guid UserId { get; set; }


    }
}
