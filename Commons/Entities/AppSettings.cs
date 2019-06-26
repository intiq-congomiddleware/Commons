using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Commons.Entities
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
        public string[] loggerModeOn { get; set; }
        public string PaytSproc { get; set; }
        public string ChrgsSproc { get; set; }
        public string ChrgsSproc2 { get; set; }
        public string UserId { get; set; }
        public int Duration { get; set; }
    }
}
