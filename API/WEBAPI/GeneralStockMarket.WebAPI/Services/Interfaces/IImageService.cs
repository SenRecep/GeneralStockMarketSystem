
using System.Threading.Tasks;

using GeneralStockMarket.CoreLib.Response;

using Microsoft.AspNetCore.Http;

namespace GeneralStockMarket.WebAPI.Services.Interfaces
{
    public interface IImageService
    {
        public Task<Response<string>> UploadImageAsync(IFormFile formFile);
    }
}
