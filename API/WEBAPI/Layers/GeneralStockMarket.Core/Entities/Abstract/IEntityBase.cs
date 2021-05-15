using System;

namespace GeneralStockMarket.Core.Entities.Abstract
{
    public interface IEntityBase
    {
        public Guid Id { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public Guid CreatedUserId { get; set; }
        public Guid? UpdateUserId { get; set; }
        public bool IsDeleted { get; set; }
    }
}
