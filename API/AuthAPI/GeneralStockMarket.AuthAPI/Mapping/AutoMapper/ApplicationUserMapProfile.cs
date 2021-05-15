
using AutoMapper;

using GeneralStockMarket.AuthAPI.Dtos;
using GeneralStockMarket.AuthAPI.Models;

namespace GeneralStockMarket.AuthAPI.Mapping.AutoMapper
{
    public class ApplicationUserMapProfile : Profile
    {
        public ApplicationUserMapProfile()
        {
            CreateMap<ApplicationUser, ApplicationUserDto>();
        }
    }
}
