using System;

using GeneralStockMarket.DTO.Request.Enums;

namespace GeneralStockMarket.DTO.Request
{
    public class GenaralCreateDto
    {
        public string Description { get; set; }
        public double Amount { get; set; }
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public RequestType  RequestType { get; set; }
        public MoneyType MoneyType { get; set; }
        public Guid UserId { get; set; }

    }
}
