using System;
using System.Collections.Generic;

using GeneralStockMarket.CoreLib.Interfaces;
using GeneralStockMarket.DTO.MerketItem;
using GeneralStockMarket.DTO.ProductItem;

namespace GeneralStockMarket.DTO.Wallet
{
    public class WalletDto : IDTO
    {
        public Guid Id { get; set; }
        public double Money { get; set; }
        public List<ProductItemDto> ProductItems { get; set; }
        public List<MarketItemDto> MarketItems { get; set; }
    }
}
