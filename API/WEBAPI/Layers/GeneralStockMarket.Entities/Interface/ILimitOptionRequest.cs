
using System;

using GeneralStockMarket.Core.Entities.Abstract;

namespace GeneralStockMarket.Entities.Interface
{
    public interface ILimitOptionRequest : IEntityBase
    {
        public Guid WalletId { get; set; }
        public Guid ProductId { get; set; }
        public double Amount { get; set; }
        public double Money { get; set; }
        public double UnitPrice { get; set; }
        public bool InProgress { get; set; }
    } 
}
