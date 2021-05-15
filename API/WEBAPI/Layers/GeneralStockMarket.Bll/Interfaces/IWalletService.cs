using System;
using System.Threading.Tasks;

using GeneralStockMarket.DTO.Wallet;

namespace GeneralStockMarket.Bll.Interfaces
{
    public interface IWalletService
    {
        public Task<WalletDto> GetWalletByUserIdAsync(Guid id);
        public Task<bool> WalletIsExistByUserIdAsync(Guid id);
    }
}
