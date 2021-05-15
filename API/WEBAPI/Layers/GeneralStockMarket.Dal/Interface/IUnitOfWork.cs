using System;
using System.Threading.Tasks;

namespace GeneralStockMarket.Dal.Concrete.EntityFrameworkCore.Repositories
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        public Task<bool> Commit(bool state = true);
    }
}
