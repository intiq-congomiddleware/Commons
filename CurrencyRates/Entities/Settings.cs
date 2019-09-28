using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyRates.Entities
{
    public class Settings
    {
        public string BranchCode { get; set; }
        public string[] CCY1 { get; set; }
        public string CCY2 { get; set; }
        public string RateType { get; set; }
        public string IntAuthStat { get; set; }
    }
}
