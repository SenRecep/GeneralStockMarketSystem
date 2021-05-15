using System;
using System.Threading.Tasks;

using AutoMapper;

using GeneralStockMarket.Bll.Interfaces;
using GeneralStockMarket.Dal.Interface;
using GeneralStockMarket.DTO.Wallet;

namespace GeneralStockMarket.Bll.Managers
{
    public class WalletManager : IWalletService
    {
        private readonly IWalletRepository walletRepository;
        private readonly IMapper mapper;

        public WalletManager(IWalletRepository walletRepository,IMapper mapper)
        {
            this.walletRepository = walletRepository;
            this.mapper = mapper;
        }
        public async Task<WalletDto> GetWalletByUserIdAsync(Guid id)
        {
            var wallet = await walletRepository.GetWalletByUserIdAsync(id);
            var dto = mapper.Map<WalletDto>(wallet);
            return dto;
        }

        public async Task<bool> WalletIsExistByUserIdAsync(Guid id)
        {
            return await walletRepository.WalletIsExistByUserIdAsync(id);
        }
    }
}
