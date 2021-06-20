using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AutoMapper;

using GeneralStockMarket.Bll.Interfaces;
using GeneralStockMarket.Core.Entities.Abstract;
using GeneralStockMarket.CoreLib.Interfaces;
using GeneralStockMarket.Dal.Interface;

namespace GeneralStockMarket.Bll.Managers
{
    public class GenericManager<T> : IGenericService<T>
        where T : class, IEntityBase, new()
    {
        private readonly IGenericRepository<T> genericRepository;
        private readonly IMapper mapper;
        private readonly ICustomMapper customMapper;

        public GenericManager(IGenericRepository<T> genericRepository, IMapper mapper, ICustomMapper customMapper)
        {
            this.genericRepository = genericRepository;
            this.mapper = mapper;
            this.customMapper = customMapper;
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

        public async Task<D> GetByUserIdAsync<D>(Guid id)
        {
            T entity = await genericRepository.GetByUserIdAsync(id);
            D result = mapper.Map<D>(entity);
            return result;
        }

        public async Task RemoveAsync<D>(D dto, bool hardDelete = false) where D : IDTO
        {
            T dummyEntity = mapper.Map<T>(dto);
            T orjinal = await genericRepository.GetByIdAsync(dummyEntity.Id);
            orjinal = customMapper.Map(dto, orjinal);
            await genericRepository.RemoveAsync(orjinal, hardDelete);
        }

        public async Task UpdateAsync<D>(D dto) where D : IDTO
        {
            T dummyEntity = mapper.Map<T>(dto);
            T orjinal = await genericRepository.GetByIdAsync(dummyEntity.Id);
            orjinal = customMapper.Map(dto, orjinal);
            await genericRepository.UpdateAsync(orjinal);
        }

        public async Task<bool> Commit(bool state = true) => await genericRepository.Commit(state);

        public void BeginTransaction()
        {
            genericRepository.BeginTransaction();
        }
    }
}
