using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using GeneralStockMarket.ApiShared.ControllerBases;
using GeneralStockMarket.ApiShared.Services.Interfaces;
using GeneralStockMarket.CoreLib.Response;
using GeneralStockMarket.Dal.Interface;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using OfficeOpenXml;
using OfficeOpenXml.Table;

namespace GeneralStockMarket.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExportController : CustomControllerBase
    {
        private readonly ISharedIdentityService sharedIdentityService;
        private readonly ITransactionRepository transactionRepository;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ExportController(ISharedIdentityService sharedIdentityService, ITransactionRepository transactionRepository, IWebHostEnvironment webHostEnvironment)
        {
            this.sharedIdentityService = sharedIdentityService;
            this.transactionRepository = transactionRepository;
            this.webHostEnvironment = webHostEnvironment;
        }
        [HttpGet]
        public async Task<IActionResult> Export()
        {
            Guid guid = Guid.NewGuid();
            string fileName = $"{guid}.xlsx";
            string path = Path.Combine(webHostEnvironment.WebRootPath, "Exports", fileName);
            FileStream fileStream = new FileStream(path, FileMode.Create);
            List<Entities.ComplexTypes.ExportModel> export = await transactionRepository.GetAllAsync(Guid.Parse(sharedIdentityService.GetUserId));
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excelPackage = new(fileStream);
            ExcelWorksheet excelBlank = excelPackage.Workbook.Worksheets.Add("Alım/Satım");
            excelBlank.Cells["A1"].LoadFromCollection(export, true, TableStyles.Light15);
            excelBlank.Cells[1, 1].Value = "İşlem Tarihi";
            excelBlank.Cells[1, 2].Value = "Ürün Adı";
            excelBlank.Cells[1, 3].Value = "Birim Fiyatı";
            excelBlank.Cells[1, 4].Value = "Adet";
            excelBlank.Cells[1, 5].Value = "İşlem Tipi";
            await excelPackage.SaveAsync();
            fileStream.Close();
            return CreateResponseInstance(Response<string>.Success(fileName, StatusCodes.Status201Created));
        }

        [HttpGet("{fileName}")]
        [AllowAnonymous]
        public IActionResult Download(string fileName)
        {
            string path = Path.Combine(webHostEnvironment.WebRootPath, "Exports", fileName);
            FileStream fileStream = System.IO.File.OpenRead(path);
            return File(fileStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Hesap Hareketleri.xlsx");
        }
    }
}
