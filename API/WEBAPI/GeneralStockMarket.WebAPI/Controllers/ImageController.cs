using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using GeneralStockMarket.ApiShared.ControllerBases;
using GeneralStockMarket.CoreLib.ExtensionMethods;
using GeneralStockMarket.CoreLib.Response;
using GeneralStockMarket.CoreLib.StringInfo;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GeneralStockMarket.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ImageController : CustomControllerBase
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly ILogger<ImageController> logger;

        public ImageController(IWebHostEnvironment webHostEnvironment, ILogger<ImageController> logger)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.logger = logger;
        }

        ///<summary>
        ///Görsel yüklemek için kullanılır.
        ///</summary>  
        ///<response code="201">Başarıyla yüklendi.</response>
        ///<response code="400">Dosya bulunamadı ya da desteklenmiyor.</response>
        ///<response code="500">Ürün resmi yüklenirken hata ile karşılaşıldı</response>
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
            {
                var response = Response<NoContent>.Fail(
                            statusCode: StatusCodes.Status400BadRequest,
                            isShow: true,
                            path: "api/image/save",
                            errors: "desteklenmeyen dosya türü"
                        );
                logger.LogResponse(response);
                return CreateResponseInstance(response);
            }


            string uniqFileName = $"{Guid.NewGuid()}{Path.GetExtension(formFile.FileName)}";
            string folderPath = Path.Combine(webHostEnvironment.WebRootPath, ImageInfo.ProductImages);
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
            string path = Path.Combine(webHostEnvironment.WebRootPath, ImageInfo.ProductImages, uniqFileName);

            try
            {
                using FileStream fileStream = new(path, FileMode.Create);
                await formFile.CopyToAsync(fileStream, cancellationToken);
                var response = Response<string>.Success(uniqFileName, StatusCodes.Status201Created);
                logger.LogResponse(response,"Fotoğraf yüklendi");
                return CreateResponseInstance(response);
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
        ///<summary>
        ///İsmi verilen fotoğrafı silme.
        ///</summary>  
        ///<response code="204">Başarıyla silindi.</response>
        ///<response code="404">Dosya bulunamadı.</response>
        ///<response code="500">Ürün resmi silinirken hata ile karşılaşıldı</response>
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
