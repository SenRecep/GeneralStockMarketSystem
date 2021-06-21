using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

using GeneralStockMarket.ClientShared.Services.Interfaces;
using GeneralStockMarket.CoreLib.Response;

namespace GeneralStockMarket.ClientShared.Services
{
    public class ExportService : IExportService
    {
        private readonly HttpClient httpClient;

        public ExportService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public Task<Response<string>> GetExportFileAsync() => httpClient.GetFromJsonAsync<Response<string>>("api/export");

    }
}
