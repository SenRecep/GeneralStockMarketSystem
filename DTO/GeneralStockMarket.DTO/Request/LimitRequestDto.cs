using System;

namespace GeneralStockMarket.DTO.Request
{
    public class LimitRequestDto
    {
        public Guid Id { get; set; }
        public DateTime CreatedTime { get; set; }
        public double Amount { get; set; }
        public double Money { get; set; }
        public double UnitPrice { get; set; }
        public bool InProgress { get; set; }
        public string ProductName { get; set; }
    }
}
