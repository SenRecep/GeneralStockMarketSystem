using System.Threading.Tasks;

using GeneralStockMarket.CoreLib.Response;

namespace GeneralStockMarket.ClientShared.Services.Interfaces
{
    public interface IExportService
    {
        public Task<Response<string>> GetExportFileAsync();
    }
}
