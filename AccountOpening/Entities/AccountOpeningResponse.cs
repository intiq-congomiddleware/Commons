using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountOpening.Entities
{
    public class AccountOpeningResponse
    {
        public bool status { get; set; }
        public List<string> message { get; set; }
    }
    public class AccountOpeningResponse2
    {
        public bool status { get; set; }
        public Dictionary<string, string> message { get; set; }
    }
}
