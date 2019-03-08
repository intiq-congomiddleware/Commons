using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountOpening.Entities
{
    public class AccountOpeningResponse
    {
        public string CUSTOMER_NO { get; set; }
        public string ACCOUNT_NO { get; set; }
        public string CUSTOMER_NAME { get; set; }
        public string BRANCH_CODE { get; set; }
    }
}
