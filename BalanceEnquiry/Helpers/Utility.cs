using BalanceEnquiry.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BalanceEnquiry.Helpers
{
    public static class Utility
    {
        public static BalanceEnquiryResponse GetResponse_(ModelStateDictionary ModelState)
        {
            List<string> errormsg = new List<string>();
            foreach (var modelState in ModelState.Values)
            {
                foreach (var modelError in modelState.Errors)
                {
                    if (!string.IsNullOrEmpty(modelError.ErrorMessage))
                        errormsg.Add(modelError.ErrorMessage);
                }
            }
            return new BalanceEnquiryResponse()
            {
                status = false,
                message = errormsg
            };
        }
        public static BalanceEnquiryResponse2 GetResponse(ModelStateDictionary ModelState)
        {
            var errormsg = new Dictionary<string, string>();
            foreach (var modelState in ModelState.Values)
            {
                foreach (var modelError in modelState.Errors)
                {
                    if (!string.IsNullOrEmpty(modelError.ErrorMessage))
                        errormsg.Add(modelState.AttemptedValue, modelError.ErrorMessage);
                }
            }

            return new BalanceEnquiryResponse2()
            {
                status = false,
                message = errormsg
            };
        }
        public static BalanceEnquiryResponse GetResponse(Exception ex)
        {
            Console.WriteLine(ex.ToString());
            List<string> errormsg = new List<string>();
            errormsg.Add(ex.Message);

            return new BalanceEnquiryResponse()
            {
                status = false,
                message = errormsg
            };
        }
        public static BalanceEnquiryResponse GetResponse(Exception ex, HttpStatusCode statuscode)
        {
            Console.WriteLine(ex.ToString());
            List<string> errormsg = new List<string>();
            errormsg.Add(ex.Message);

            return new BalanceEnquiryResponse()
            {
                status = false,
                message = errormsg
            };
        }
        public static BalanceEnquiryResponse GetResponse(string msg, HttpStatusCode statuscode)
        {
            Console.WriteLine(msg);
            List<string> errormsg = new List<string>();
            errormsg.Add(msg);

            return new BalanceEnquiryResponse()
            {
                status = false,
                message = errormsg
            };
        }

        public static string GetReferenceNo()
        {
            string rand = string.Empty;
            try
            {
                CultureInfo ci = CultureInfo.InvariantCulture;
                var refNo = new Random(Guid.NewGuid().GetHashCode());
                string referenceNo = "";// refNo.Next(9).ToString(CultureInfo.InvariantCulture).Trim();
                string datetimeStamp = DateTime.UtcNow.ToString("ddMMyyyyHHmmssf", ci);
                rand = referenceNo + datetimeStamp;

            }
            catch (Exception ex)
            {
                Console.WriteLine("GetReferenceNo", ex.Message + "" + ex.StackTrace, "");
            }
            return rand;
        }
    }
}
