using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using GeneralStockMarket.Core.Entities.Abstract;
using GeneralStockMarket.CoreLib.Interfaces;
using GeneralStockMarket.Entities.Interface;

namespace GeneralStockMarket.Bll.Interfaces
{
    public interface IGenericService<T>
        where T : class, IEntityBase, new()
    {
        public Task<IEnumerable<D>> GetAllAsync<D>() where D : IDTO;
        public Task<IEnumerable<D>> GetAllWithDeletedAsync<D>() where D : IDTO;
        public Task<IEnumerable<D>> GetAllByUserIdAsync<D>(Guid id) where D : IDTO;

        public Task<D> GetByIdAsync<D>(Guid id) where D : IDTO;

        public Task<T> AddAsync<D>(D dto) where D : IDTO;
        public Task UpdateAsync<D>(D dto) where D : IDTO;

        public Task RemoveAsync<D>(D dto, bool hardDelete = false) where D : IDTO;

        public Task<bool> Commit(bool state = true);
    }
}
