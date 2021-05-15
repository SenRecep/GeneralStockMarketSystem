
using AutoMapper;

using GeneralStockMarket.ClientShared.Models;

using IdentityModel.Client;

namespace GeneralStockMarket.ClientShared.AutoMapperProfile.Mapping
{
    public class IdentityResponseGeneralProfile : Profile
    {
        public IdentityResponseGeneralProfile()
        {
            CreateMap<TokenResponse, Token>();
        }
    }
}
