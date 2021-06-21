using System;

namespace GeneralStockMarket.Entities.ComplexTypes
{
    /// <summary>
    /// TransactionType=true Saller
    /// TransactionType=false Buyyer
    /// </summary>
    public record ExportModel(string CreatedTime, string Name, double UnitPrice, double Amount, string TransactionType);
}
