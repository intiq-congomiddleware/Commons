using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BalanceEnquiry.Entities
{
    public class BalanceEnquiryRequest
    {
        public string accountNumber { get; set; }
        [JsonIgnore]
        public string userId { get; set; }
    }
}
