using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Commons.Entities
{
    public class Constant
    {
        public const string TIMESTAMP_FORMAT_1 = "yyyy-MM-dd HH:mm:ss:fff";
        public const string TIMESTAMP_FORMAT_2 = "yyyyMMddHHmmss";

        public const string STAT_NO_CR = "Credit Account has Restrictions.";
        public const string STAT_NO_DR = "Debit Account has Restrictions.";
        public const string ACCOUNT_NOT_LINKED = "Accounts might not be linked to same Customer.";
    }
}
