using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AutoMapper;

using GeneralStockMarket.Bll.Interfaces;
using GeneralStockMarket.Core.Entities.Abstract;
using GeneralStockMarket.CoreLib.Interfaces;
using GeneralStockMarket.Dal.Interface;
using GeneralStockMarket.Entities.Interface;

namespace GeneralStockMarket.Bll.Managers
{
    public class GenericManager<T> : IGenericService<T>
        where T : class, IEntityBase, new()
    {
        private readonly IGenericRepository<T> genericRepository;
        private readonly IMapper mapper;

        public GenericManager(IGenericRepository<T> genericRepository, IMapper mapper)
        {
            this.genericRepository = genericRepository;
            this.mapper = mapper;
        }

        public async Task<T> AddAsync<D>(D dto)
           where D : IDTO
        {
            T entity = mapper.Map<T>(dto);
            await genericRepository.AddAsync(entity);
            return entity;
        }

        public async Task<IEnumerable<D>> GetAllAsync<D>() where D : IDTO
        {
            IEnumerable<T> entities = await genericRepository.GetAllAsync();
            IEnumerable<D> result = mapper.Map<IEnumerable<D>>(entities);
            return result;
        }

        public async Task<IEnumerable<D>> GetAllWithDeletedAsync<D>() where D : IDTO
        {
            IEnumerable<T> entities = await genericRepository.GetAllAsync();
            IEnumerable<D> result = mapper.Map<IEnumerable<D>>(entities);
            return result;
        }

        public async Task<IEnumerable<D>> GetAllByUserIdAsync<D>(Guid id) where D : IDTO
        {
            IEnumerable<T> entities = await genericRepository.GetAllByUserIdAsync(id);
            IEnumerable<D> result = mapper.Map<IEnumerable<D>>(entities);
            return result;
        }

        public async Task<D> GetByIdAsync<D>(Guid id) where D : IDTO
        {
            T entity = await genericRepository.GetByIdAsync(id);
            D result = mapper.Map<D>(entity);
            return result;
        }

        public async Task RemoveAsync<D>(D dto, bool hardDelete = false) where D : IDTO
        {
            T entity = mapper.Map<T>(dto);
            var deletedItem = await genericRepository.GetByIdAsync(entity.Id);
            await genericRepository.RemoveAsync(deletedItem, hardDelete);
        }

        public async Task UpdateAsync<D>(D dto) where D : IDTO
        {
            T entity = mapper.Map<T>(dto);
            await genericRepository.UpdateAsync(entity);
        }

        public async Task<bool> Commit(bool state = true) => await genericRepository.Commit(state);
    }
}
