using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BalanceEnquiry.Entities
{
    public class AppSettings
    {
        public string FlexSchema { get; set; }
        public string TMESchema { get; set; }
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int Expires { get; set; }
        public string FlexConnection { get; set; }
        public string TMEConnection { get; set; }
    }
}
