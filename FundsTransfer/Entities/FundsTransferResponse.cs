using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FundsTransfer.Entities
{
    public class FundsTransferResponse
    {
        public string response_code { get; set; }
        public string response_msg { get; set; }
        public string actualtrnamt { get; set; }
        public string rate { get; set; }
        public string trnrefno { get; set; }
        public string id { get; set; }
    }
}
