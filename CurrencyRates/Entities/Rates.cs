using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyRates.Entities
{
    public class Rates
    {
        public string branch_code { get; set; }
        public string ccy1 { get; set; }
        public string ccy2 { get; set; }
        public string rate_type { get; set; }
        public decimal rate { get; set; }
        public string rate_date { get; set; }
        public string int_auth_stat { get; set; }
    }
}
