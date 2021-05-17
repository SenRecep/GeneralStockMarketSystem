using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using GeneralStockMarket.Bll.Interfaces;
using GeneralStockMarket.Dal.Interface;
using GeneralStockMarket.DTO.Product;
using GeneralStockMarket.Entities.Concrete;

namespace GeneralStockMarket.Bll.Managers
{
    public class ProductManager : IProductService
    {
        private readonly IProductRepository productRepository;
        private readonly IMapper mapper;

        public ProductManager(IProductRepository productRepository, IMapper mapper)
        {
            this.productRepository = productRepository;
            this.mapper = mapper;
        }

        public async Task<List<ProductDto>> GetProductsAsync()
        {
            List<ProductDto> result = new();
            List<Product> products = await productRepository.GetProductsAsync();

            foreach (Product product in products)
            {
                ProductDto productDto = mapper.Map<ProductDto>(product);
                if (product.MarketItems.Any())
                {
                    productDto.Amount = product.MarketItems.Sum(x => x.Amount);
                    productDto.AvgPrice = product.MarketItems.Select(x => x.UnitPrice*x.Amount).Sum(x=>x) / productDto.Amount;
                }
                result.Add(productDto);
            }

            return result;
        }

        public async Task<ProductTradeDto> GetProductByIdAsync(Guid id)
        {
            Product product = await productRepository.GetProductByIdAsync(id);
            ProductTradeDto dto = mapper.Map<ProductTradeDto>(product);
            if (product.MarketItems.Any())
            {
                dto.Amount = dto.MarketItems.Sum(x => x.Amount);
                dto.AvgPrice = dto.MarketItems.Select(x => x.UnitPrice * x.Amount).Sum(x => x) / dto.Amount;
            }
            return dto;
        }
    }
}
