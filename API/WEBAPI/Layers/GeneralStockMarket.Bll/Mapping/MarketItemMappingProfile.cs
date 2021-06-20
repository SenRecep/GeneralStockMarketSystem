
using AutoMapper;

using GeneralStockMarket.DTO.MerketItem;
using GeneralStockMarket.Entities.Concrete;

namespace GeneralStockMarket.Bll.Mapping
{
    public class MarketItemMappingProfile : Profile
    {
        public MarketItemMappingProfile()
        {
            CreateMap<MarketItem, MarketItemDto>();
            CreateMap<MarketItem, MarketItemUpdateDto>();
            CreateMap<MarketItemDto, MarketItemUpdateDto>();
            CreateMap<MarketItemUpdateDto, MarketItem>();

        }
    }
}
