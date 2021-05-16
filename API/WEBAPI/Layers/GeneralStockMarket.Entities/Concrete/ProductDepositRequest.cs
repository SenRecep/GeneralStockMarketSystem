using System;

using GeneralStockMarket.Core.Entities.Concrete;
using GeneralStockMarket.Entities.Interface;

namespace GeneralStockMarket.Entities.Concrete
{
    public class ProductDepositRequest : EntityBase, IProductDepositRequest
    {
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public string Description { get; set; }
        public bool? Verify { get; set; }
        public Guid UserId { get; set; }
        public double Amount { get; set; }
    }
}
