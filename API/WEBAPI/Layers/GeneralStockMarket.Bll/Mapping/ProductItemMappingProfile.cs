
using AutoMapper;

using GeneralStockMarket.DTO.ProductItem;
using GeneralStockMarket.Entities.Concrete;

namespace GeneralStockMarket.Bll.Mapping
{
    public class ProductItemMappingProfile : Profile
    {
        public ProductItemMappingProfile()
        {
            CreateMap<ProductItem, ProductItemDto>().ReverseMap();
        }
    }
}
