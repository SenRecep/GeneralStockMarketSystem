using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AutoMapper;

using GeneralStockMarket.ApiShared.ControllerBases;
using GeneralStockMarket.ApiShared.Filters;
using GeneralStockMarket.ApiShared.Services.Interfaces;
using GeneralStockMarket.Bll.Interfaces;
using GeneralStockMarket.CoreLib.ExtensionMethods;
using GeneralStockMarket.CoreLib.Response;
using GeneralStockMarket.DTO.Product;
using GeneralStockMarket.Entities.Concrete;
using GeneralStockMarket.WebAPI.Services;
using GeneralStockMarket.WebAPI.Services.Interfaces;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GeneralStockMarket.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : CustomControllerBase
    {
        private readonly IGenericService<Product> productGenericService;
        private readonly IProductService productService;
        private readonly IImageService imageService;
        private readonly IMapper mapper;
        private readonly ISharedIdentityService sharedIdentityService;
        private readonly ILogger<ProductController> logger;

        public ProductController(
            IGenericService<Product> productGenericService,
            IProductService productService,
            IImageService imageService,
            IMapper mapper,
            ISharedIdentityService sharedIdentityService,
            ILogger<ProductController> logger)
        {
            this.productGenericService = productGenericService;
            this.productService = productService;
            this.imageService = imageService;
            this.mapper = mapper;
            this.sharedIdentityService = sharedIdentityService;
            this.logger = logger;
        }
        ///<summary>
        ///Bütün ürünleri getirme.
        ///</summary>  
        ///<response code="200">Başarıyla gerçekleşti.</response>      
        [HttpGet]
        //[AllowAnonymous]
        public async Task<IActionResult> Get()
        {
            List<ProductDto> products = await productService.GetProductsAsync();
            Response<IEnumerable<ProductDto>> response = Response<IEnumerable<ProductDto>>.Success(products, StatusCodes.Status200OK);
            logger.LogInformation("api/product/getproducts calling enpoint");
            return CreateResponseInstance(response);
        }
        ///<summary>
        ///Id bilgisi verilen ürünü getirme.
        ///</summary>  
        ///<response code="200">Başarıyla gerçekleşti.</response>
        ///<response code="404">Ürün bulunamadı.</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var productTradeDto = await productService.GetProductByIdAsync(id);
            if (productTradeDto.IsNull())
            {
                logger.LogInformation("api/product/getproductbyid calling enpoint");
                logger.LogInformation("product not found");
                return CreateResponseInstance(Response<NoContent>.Fail(
                    statusCode: StatusCodes.Status404NotFound,
                    isShow: true,
                    path: "api/product/getproductbyid",
                    errors: "İstenilen ürün bulunamadı"
                    ));
            }
            Response<ProductTradeDto> response = Response<ProductTradeDto>.Success(productTradeDto, StatusCodes.Status200OK);
            logger.LogInformation("api/products/getproductbyid calling enpoint");
            return CreateResponseInstance(response);
        }
        ///<summary>
        ///Ürün oluşturma.
        ///</summary>  
        ///<response code="201">Başarıyla eklendi.</response>
        ///<response code="400">Dosya bulunamadı ya da desteklenmiyor.</response>
        ///<response code="500">Ürün resmi yüklenirken hata ile karşılaşıldı</response>
        [HttpPost("{name}")]
        //[ApiExplorerSettings(IgnoreApi= true)]
        public async Task<IActionResult> Post([FromForm] IFormFile Image, [FromRoute] string name)
        {
            Guid userId = Guid.Parse(sharedIdentityService.GetUserId);
            ProductCreateDto productCreateDto = new()
            {
                Image = Image,
                Name = name
            };
            productCreateDto.CreatedUserId = userId;
            var imageUploadResponse = await imageService.UploadImageAsync(productCreateDto.Image);
            if (!imageUploadResponse.IsSuccessful)
            {
                logger.LogResponse(imageUploadResponse,"Ürün resmi yükleme başarısız");
                return CreateResponseInstance(imageUploadResponse);
            }
                

            productCreateDto.ImageName = imageUploadResponse.Data;
            Product result = await productGenericService.AddAsync(productCreateDto);
            await productGenericService.Commit();
            Response<ProductDto> response = Response<ProductDto>.Success(mapper.Map<ProductDto>(result), StatusCodes.Status201Created);
            logger.LogInformation($"api/product/addproduct calling enpoint");
            logger.LogInformation($" adding product name='${productCreateDto.Name}'");
            return CreateResponseInstance(response);
        }
        ///<summary>
        ///Ürün güncelleme.
        ///</summary>  
        ///<response code="204">Başarıyla güncellendi.</response>
        [HttpPut]
        public async Task<IActionResult> Put(ProductUpdateDto productUpdateDto)
        {
            await productGenericService.UpdateAsync(productUpdateDto);
            await productGenericService.Commit();

            Response<NoContent> response = Response<NoContent>.Success(StatusCodes.Status204NoContent);

            logger.LogInformation($"api/product/addproduct calling enpoint");
            logger.LogInformation($" updated product name='${productUpdateDto.Name}'");
            return CreateResponseInstance(response);
        }

        ///<summary>
        ///Id bilgisi verilen ürünü silme.
        ///</summary>  
        ///<response code="204">Başarıyla silindi.</response>
        ///<response code="404">Ürün bulunamadı.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var product = await productGenericService.GetByIdAsync<ProductDto>(id);
            if (product.IsNull())
            {
                logger.LogInformation("api/product/delete calling enpoint");
                logger.LogInformation("product not found");
                return CreateResponseInstance(Response<NoContent>.Fail(
                    statusCode: StatusCodes.Status404NotFound,
                    isShow: true,
                    path: "api/product/delete",
                    errors: "İstenilen ürün bulunamadı"
                    ));
            }

            await productGenericService.RemoveAsync(product);
            await productGenericService.Commit();

            Response<NoContent> response = Response<NoContent>.Success(StatusCodes.Status204NoContent);
            logger.LogInformation("api/product/delete calling enpoint");
            return CreateResponseInstance(response);
        }
    }
}
