using System;

using GeneralStockMarket.CoreLib.ExtensionMethods;
using GeneralStockMarket.DTO.Request.Enums;

namespace GeneralStockMarket.DTO.Response
{
    public class ExchangeResponse
    {
        public Motd Motd { get; set; }
        public bool Success { get; set; }
        public string Base { get; set; }
        public string Date { get; set; }
        public Rates Rates { get; set; }
    }

    public class Motd
    {
        public string Msg { get; set; }
        public string Url { get; set; }
    }

    public class Rates
    {
        public float TRY { get; set; }
        public float EUR { get; set; }
        public float GBP { get; set; }
        public float USD { get; set; }
        public float GetMoney(MoneyType moneyType) => typeof(Rates).GetProperty(Enum.GetName(moneyType)).GetValue(this).Cast<float>();
    }
}
