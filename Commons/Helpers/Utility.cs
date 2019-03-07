using Commons.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Commons.Helpers
{
    public static class Utility
    {
        
        public static Response2 GetResponse(ModelStateDictionary ModelState)
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

            return new Response2()
            {
                status = false,
                message = errormsg
            };
        }
        public static Response GetResponse(Exception ex)
        {
            Console.WriteLine(ex.ToString());         

            return new Response()
            {
                status = false,
                message = ex.Message
            };
        }
        public static Response GetResponse(Exception ex, HttpStatusCode statuscode)
        {
            Console.WriteLine(ex.ToString());

            return new Response()
            {
                status = false,
                message = ex.Message
            };
        }
        public static Response GetResponse(string msg, HttpStatusCode statuscode)
        {
            Console.WriteLine(msg);           

            return new Response()
            {
                status = false,
                message = msg
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

        public static string RandomString(int length)
        {
            char[] chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            byte[] data = new byte[1];

            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetNonZeroBytes(data);
                data = new byte[length];
                crypto.GetNonZeroBytes(data);
            }

            StringBuilder result = new StringBuilder(length);

            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }

            return result.ToString();
        }
    }
}
