
using AutoMapper;

using GeneralStockMarket.DTO.Request;
using GeneralStockMarket.Entities.Concrete;

namespace GeneralStockMarket.Bll.Mapping
{
    public class RequestMappingProfile : Profile
    {
        public RequestMappingProfile()
        {
            CreateMap<DepositRequest, DepositRequestDto>().ReverseMap();
            CreateMap<NewTypeRequest, NewTypeRequestDto>().ReverseMap();
            CreateMap<ProductDepositRequest, ProductDepositRequestDto>().ReverseMap();
        }
    }
}
