using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using GeneralStockMarket.Entities.ComplexTypes;

namespace GeneralStockMarket.Dal.Interface
{
    public interface ITransactionRepository
    {
        Task<List<ExportModel>> GetAllAsync(Guid UserId);
    }
}
