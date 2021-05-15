
using AutoMapper;

using GeneralStockMarket.DTO.Product;
using GeneralStockMarket.Entities.Concrete;

namespace GeneralStockMarket.Bll.Mapping
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<Product, ProductCreateDto>().ReverseMap();
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Product, ProductUpdateDto>().ReverseMap();

            CreateMap<Product,ProductTradeDto>();
        }
    }
}
