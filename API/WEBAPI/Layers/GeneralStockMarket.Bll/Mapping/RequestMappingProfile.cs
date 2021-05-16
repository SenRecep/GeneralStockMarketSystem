
using AutoMapper;

using GeneralStockMarket.DTO.General;
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


            CreateMap<DeleteDto, DepositRequest>();
            CreateMap<DeleteDto, NewTypeRequest>();
            CreateMap<DeleteDto, ProductDepositRequest>();


            CreateMap<VerifyDto, DepositRequest>();
            CreateMap<VerifyDto, NewTypeRequest>();
            CreateMap<VerifyDto, ProductDepositRequest>();


            CreateMap<DepositRequest, CreateDepositRequest>().ReverseMap();
            CreateMap<NewTypeRequest, CreateNewTypeRequest>().ReverseMap();
            CreateMap<ProductDepositRequest, CreateProductDepositRequest>().ReverseMap();

            CreateMap<GenaralCreateDto,CreateDepositRequest>();
            CreateMap<GenaralCreateDto,CreateProductDepositRequest>();
            CreateMap<GenaralCreateDto,CreateNewTypeRequest>();
        }
    }
}
