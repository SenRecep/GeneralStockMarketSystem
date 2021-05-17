using System;

using GeneralStockMarket.CoreLib.Interfaces;

namespace GeneralStockMarket.DTO.Transaction
{
    public class TransactionCreateDto : IDTO
    {
        public Guid WalletIdSeller { get; set; }
        public Guid WalletIdBuyer { get; set; }
        public double UnitPrice { get; set; }
        public double Amount { get; set; }
        public Guid ProductId { get; set; }
        public Guid CreatedUserId { get; set; }
    }
}
