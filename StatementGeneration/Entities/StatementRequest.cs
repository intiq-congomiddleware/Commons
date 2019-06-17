using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StatementGeneration.Entities
{
    public class StatementRequest
    {
        public string accountNumber { get; set; }
        public int noOfRecords { get; set; }
        public string userId { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
    }
    public class StatementRequestDTO
    {
        public string acct { get; set; }
        public string USERID { get; set; }
        public DateTime start_dt { get; set; }
        public DateTime end_dt { get; set; }
    }
}
