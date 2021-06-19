using System;

using GeneralStockMarket.Core.Entities.Concrete;
using GeneralStockMarket.Entities.Interface;

namespace GeneralStockMarket.Entities.Concrete
{
    public class DepositRequest : EntityBase, IDepositRequest
    {
        public string Description { get; set; }
        public bool? Verify { get; set; }
        public Guid UserId { get; set; }
        public double Amount { get; set; }
        public short MoneyType { get; set; }
    }
}
