using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;

using GeneralStockMarket.Bll.Models;
using GeneralStockMarket.DTO.Trade;

namespace GeneralStockMarket.Bll.Mapping
{
    public class TradeMappingProfile:Profile
    {
        public TradeMappingProfile()
        {
            CreateMap<TradeCreateDto,SellModel>();
            CreateMap<TradeCreateDto,BuyModel>();
        }
    }
}
