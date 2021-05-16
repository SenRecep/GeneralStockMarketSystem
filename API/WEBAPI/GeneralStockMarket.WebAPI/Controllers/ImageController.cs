using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using GeneralStockMarket.ApiShared.ControllerBases;
using GeneralStockMarket.CoreLib.Response;
using GeneralStockMarket.CoreLib.StringInfo;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GeneralStockMarket.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ImageController : CustomControllerBase
    {
        private readonly IWebHostEnvironment webHostEnvironment;

        public ImageController(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
        }
        [HttpPost]
        public async Task<IActionResult> Save(IFormFile formFile, CancellationToken cancellationToken)
        {
            if (formFile.Length <= 0)
                return CreateResponseInstance(Response<NoContent>.Fail(
                        statusCode: StatusCodes.Status400BadRequest,
                        isShow: true,
                        path: "api/image/save",
                        errors: "image not found"
                    ));


            string extension = Path.GetExtension(formFile.FileName);

            if (!ImageInfo.SupportedImageExtensions.Contains(extension, StringComparison.InvariantCultureIgnoreCase))
                return CreateResponseInstance(Response<NoContent>.Fail(
                            statusCode: StatusCodes.Status400BadRequest,
                            isShow: true,
                            path: "api/image/save",
                            errors: "desteklenmeyen dosya türü"
                        ));

            string uniqFileName = $"{Guid.NewGuid()}{Path.GetExtension(formFile.FileName)}";
            string folderPath = Path.Combine(webHostEnvironment.WebRootPath, ImageInfo.ProductImages);
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
            string path = Path.Combine(webHostEnvironment.WebRootPath, ImageInfo.ProductImages, uniqFileName);

            try
            {
                using FileStream fileStream = new(path, FileMode.Create);
                await formFile.CopyToAsync(fileStream, cancellationToken);
                return CreateResponseInstance(Response<string>.Success(uniqFileName, StatusCodes.Status201Created));
            }
            catch (Exception ex)
            {
                return CreateResponseInstance(Response<NoContent>.Fail(
                        statusCode: StatusCodes.Status500InternalServerError,
                        isShow: true,
                        path: "api/image/save",
                        errors: new[] { "Ürün resmi yüklenirken hata ile karşılaşıldı", ex.Message }
                    ));
            }
        }
        [HttpDelete("{imageName}")]
        public IActionResult Delete(string imageName)
        {
            string path = Path.Combine(webHostEnvironment.WebRootPath, ImageInfo.ProductImages, imageName);

            if (!System.IO.File.Exists(path))
                return CreateResponseInstance(Response<NoContent>.Fail(
                       statusCode: StatusCodes.Status404NotFound,
                       isShow: true,
                       path: "api/image/delete",
                       errors: "Ürün resmi bulunamadı"
                   ));

            try
            {
                System.IO.File.Delete(path);
                return CreateResponseInstance(Response<NoContent>.Success(StatusCodes.Status204NoContent));
            }
            catch (Exception ex)
            {
                return CreateResponseInstance(Response<NoContent>.Fail(
                        statusCode: StatusCodes.Status500InternalServerError,
                        isShow: true,
                        path: "api/image/delete",
                        errors: new[] { "Ürün resmi silinirken bir hata ile karşılaşıldı", ex.Message }
                    ));
            }
        }

    }
}
