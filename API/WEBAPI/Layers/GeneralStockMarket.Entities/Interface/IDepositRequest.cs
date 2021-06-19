namespace GeneralStockMarket.Entities.Interface
{
    public interface IDepositRequest : IRequest
    {
         short MoneyType { get; set; }
    }
}
