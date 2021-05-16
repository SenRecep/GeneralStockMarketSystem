using System;

using GeneralStockMarket.CoreLib.Interfaces;

namespace GeneralStockMarket.DTO.Wallet
{
    public class WalletUpdateDto : IDTO
    {
        public Guid Id { get; set; }
        public double Money { get; set; }
        public Guid UpdateUserId { get; set; }
    }
}
