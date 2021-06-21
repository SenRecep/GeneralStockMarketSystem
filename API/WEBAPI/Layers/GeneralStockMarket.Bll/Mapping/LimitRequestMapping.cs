
using AutoMapper;

using GeneralStockMarket.DTO.Request;
using GeneralStockMarket.Entities.Concrete;

namespace GeneralStockMarket.Bll.Mapping
{
    public class LimitRequestMapping : Profile
    {
        public LimitRequestMapping()
        {
            CreateMap<LimitOptionRequest, LimitRequestDto>()
                .ForMember(x => x.ProductName, opt => opt.MapFrom(dest => dest.Product.Name));
        }
    }
}
