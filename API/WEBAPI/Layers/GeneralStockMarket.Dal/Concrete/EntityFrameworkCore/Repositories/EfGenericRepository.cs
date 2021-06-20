using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using GeneralStockMarket.Core.Entities.Abstract;
using GeneralStockMarket.Dal.Interface;
using GeneralStockMarket.Entities.Interface;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace GeneralStockMarket.Dal.Concrete.EntityFrameworkCore.Repositories
{
    public class EfGenericRepository<T> : IGenericRepository<T>
        where T : class, IEntityBase, new()

    {
        private readonly DbContext dbContext;
        private readonly DbSet<T> table;

        private IDbContextTransaction dbContextTransaction;

        public EfGenericRepository(DbContext dbContext)
        {
            this.dbContext = dbContext;
            table = dbContext.Set<T>();
            BeginTransaction();
        }

        #region GetAll
        public async Task<IEnumerable<T>> GetAllAsync() => await table.Where(x => !x.IsDeleted).ToListAsync();

        public async Task<IEnumerable<T>> GetAllWithDeletedAsync() => await table.ToListAsync();

        public async Task<IEnumerable<T>> GetAllByUserIdAsync(Guid id) => await table.Where(x => !x.IsDeleted && ((IUserDependent)x).UserId == id).ToListAsync();
        #endregion

        #region Get
        public async Task<T> GetByIdAsync(Guid id) => await table.FindAsync(id);
        public async Task<T> GetByUserIdAsync(Guid id) => await table.FirstOrDefaultAsync(x => !x.IsDeleted && ((IUserDependent)x).UserId == id);
        #endregion

        #region CUD

        public async Task<T> AddAsync(T entity)
        {
            entity.CreatedTime = DateTime.Now;
            await table.AddAsync(entity);
            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            entity.UpdateTime = DateTime.Now;
            await Task.FromResult(table.Update(entity));
        }

        public async Task RemoveAsync(T entity, bool hardDelete = false)
        {
            if (hardDelete)
                await Task.FromResult(table.Remove(entity));
            else
            {
                entity.IsDeleted = true;
                await UpdateAsync(entity);
            }
        }
        #endregion

        #region Commit
        public async Task<bool> Commit(bool state = true)
        {
            if (!state)
            {
                await dbContextTransaction.RollbackAsync();
                return state;
            }

            bool commitState;
            try
            {
                await SaveChangesAsync();
                commitState = true;
            }
            catch
            {
                commitState = false;
            }

            if (commitState)
                await dbContextTransaction.CommitAsync();
            else
                await dbContextTransaction.RollbackAsync();

            return commitState;
        }
        #endregion

        #region Dispose

        public async ValueTask DisposeAsync() => await dbContext.DisposeAsync();
        #endregion

        #region Save

        public async Task<int> SaveChangesAsync() => await dbContext.SaveChangesAsync();

        public void BeginTransaction()
        {
            dbContextTransaction = dbContext.Database.BeginTransaction();
        }

        #endregion

    }
}
