﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BalanceEnquiry.Entities
{
    public class BalanceEnquiryResponse
    {
        public string cod_prod { get; set; }
        public string cod_acct_no { get; set; }
        public string bal_available { get; set; }
        public string cod_cc_brn { get; set; }
    }
    public class BalanceEnquiryResponse2
    {
        public bool status { get; set; }
        public Dictionary<string, string> message { get; set; }
    }
}
