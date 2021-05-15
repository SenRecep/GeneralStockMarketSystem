namespace GeneralStockMarket.Entities.Interface
{
    public interface IMarketItem : IItem
    {
        public double UnitPrice { get; set; }
        public bool InProgress { get; set; }
    }
}
