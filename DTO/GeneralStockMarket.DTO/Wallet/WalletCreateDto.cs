using System;

using GeneralStockMarket.CoreLib.Interfaces;

namespace GeneralStockMarket.DTO.Wallet
{
    public class WalletCreateDto : IDTO
    {
        public Guid UserId { get; set; }
        public Guid CreatedUserId { get; set; }
    }
}
