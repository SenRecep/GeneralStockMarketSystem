
using AutoMapper;

using GeneralStockMarket.DTO.Transaction;
using GeneralStockMarket.Entities.Concrete;

namespace GeneralStockMarket.Bll.Mapping
{
    public class TransactionMappingProfile : Profile
    {
        public TransactionMappingProfile()
        {
            CreateMap<Transaction, TransactionCreateDto>().ReverseMap();
        }
    }
}
