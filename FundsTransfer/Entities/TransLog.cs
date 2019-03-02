using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FundsTransfer.Entities
{
    public class TransLog
    {
        public string debitaccount { get; set; }
        public string creditaccount { get; set; }
        public string post_amt { get; set; }
        public string refno { get; set; }
        public string debit_branch { get; set; }
    }
}
