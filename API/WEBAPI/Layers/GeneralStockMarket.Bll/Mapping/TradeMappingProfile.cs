
using AutoMapper;

using GeneralStockMarket.Bll.Models;
using GeneralStockMarket.DTO.Trade;
using GeneralStockMarket.Entities.Concrete;

namespace GeneralStockMarket.Bll.Mapping
{
    public class TradeMappingProfile : Profile
    {
        public TradeMappingProfile()
        {
            CreateMap<TradeCreateDto, SellModel>();
            CreateMap<TradeCreateDto, BuyModel>();
            CreateMap<TradeCreateDto, BuybyLimitModel>().ForMember(x => x.Money, opt => opt.MapFrom(dest => dest.UnitPrice));

            CreateMap<BuybyLimitModel, LimitOptionRequest>();
        }
    }
}
