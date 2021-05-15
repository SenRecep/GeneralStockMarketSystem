using System;

using GeneralStockMarket.Core.Entities.Abstract;

namespace GeneralStockMarket.Entities.Interface
{
    public interface INewTypeRequest : IEntityBase, IUserDependent
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Verify { get; set; }
    }
}
