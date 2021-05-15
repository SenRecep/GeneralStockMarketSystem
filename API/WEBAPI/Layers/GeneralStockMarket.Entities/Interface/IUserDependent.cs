using System;

namespace GeneralStockMarket.Entities.Interface
{
    public interface IUserDependent
    {
        public Guid UserId { get; set; }
    }
}
