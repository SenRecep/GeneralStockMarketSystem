using System;

using GeneralStockMarket.Entities.Concrete;

namespace GeneralStockMarket.Entities.Interface
{
    public interface IProductDepositRequest : IRequest
    {
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
    }
}
