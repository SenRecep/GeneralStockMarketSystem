
using AutoMapper;

using GeneralStockMarket.DTO.Wallet;
using GeneralStockMarket.Entities.Concrete;

namespace GeneralStockMarket.Bll.Mapping
{
    public class WalletMappingProfile : Profile
    {
        public WalletMappingProfile()
        {
            CreateMap<Wallet, WalletDto>().ReverseMap();
            CreateMap<Wallet, WalletCreateDto>().ReverseMap();
            CreateMap<Wallet, WalletUpdateDto>().ReverseMap();
            CreateMap<WalletDto, WalletUpdateDto>().ReverseMap();
        }
    }
}
