using System;

using GeneralStockMarket.Core.Entities.Concrete;
using GeneralStockMarket.Entities.Interface;

namespace GeneralStockMarket.Entities.Concrete
{
    public class NewTypeRequest : EntityBase, INewTypeRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid UserId { get; set; }
        public bool Verify { get; set; }
    }
}
