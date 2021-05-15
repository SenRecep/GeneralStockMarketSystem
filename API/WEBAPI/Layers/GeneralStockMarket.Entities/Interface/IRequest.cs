using System;

using GeneralStockMarket.Core.Entities.Abstract;

namespace GeneralStockMarket.Entities.Interface
{
    public interface IRequest : IEntityBase, IUserDependent
    {
        public string Description { get; set; }
        public bool Verify { get; set; }
        public double Amount { get; set; }
    }
}
