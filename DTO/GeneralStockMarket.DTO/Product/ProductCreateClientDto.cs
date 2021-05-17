using System.IO;

namespace GeneralStockMarket.DTO.Product
{
    public class ProductCreateClientDto
    {
        public string Name { get; set; }
        public string FileName { get; set; }
        public Stream Image { get; set; }
    }
}
