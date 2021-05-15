using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

using GeneralStockMarket.CoreLib.Response;
using GeneralStockMarket.WebAPI.Services.Interfaces;

using Microsoft.AspNetCore.Http;

namespace GeneralStockMarket.WebAPI.Services
{
    public class ImageService : IImageService
    {
        private readonly HttpClient httpClient;

        public ImageService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<Response<string>> UploadImageAsync(IFormFile formFile)
        {
            using var form = new MultipartFormDataContent();
            using var fs = formFile.OpenReadStream();
            using var streamContent = new StreamContent(fs);
            using var fileContent = new ByteArrayContent(await streamContent.ReadAsByteArrayAsync());

            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
            form.Add(fileContent, "formFile", formFile.FileName);

            var httpresponse = await httpClient.PostAsync("api/image", form);
            var response= await httpresponse.Content.ReadFromJsonAsync<Response<string>>();
            return response;
        }
    }
}
