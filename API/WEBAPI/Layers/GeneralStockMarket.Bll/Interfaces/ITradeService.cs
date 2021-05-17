using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GeneralStockMarket.Bll.Models;

namespace GeneralStockMarket.Bll.Interfaces
{
    public interface ITradeService
    {
        public Task SellAync(SellModel sellModel);
    }
}
