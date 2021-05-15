using System;
using System.Threading.Tasks;

using GeneralStockMarket.Entities.Concrete;

namespace GeneralStockMarket.Dal.Interface
{
    public interface IWalletRepository
    {
        public Task<Wallet> GetWalletByUserIdAsync(Guid id);
        public Task<bool> WalletIsExistByUserIdAsync(Guid id);
    }
}
