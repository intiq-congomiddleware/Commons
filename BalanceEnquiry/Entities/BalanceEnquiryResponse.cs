using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BalanceEnquiry.Entities
{
    public class BalanceEnquiryResponse
    {
        public string run_userid { get; set; }
        public DateTime rundate { get; set; }
        public DateTime startdate { get; set; }
        public DateTime enddate { get; set; }
        public string accountno { get; set; }
        public decimal totaldr { get; set; }
        public decimal totalcr { get; set; }
        public decimal drcount { get; set; }
        public decimal crcount { get; set; }
        public decimal openingbalance { get; set; }
        public decimal closingbalance { get; set; }
        public string CUSTOMERNAME { get; set; }
        public string address { get; set; }
        public string currency { get; set; }
    }
    public class BalanceEnquiryResponse2
    {
        public bool status { get; set; }
        public Dictionary<string, string> message { get; set; }
    }
}
