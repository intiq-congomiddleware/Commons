using FundsTransfer.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FundsTransfer.Helpers
{
    public static class Utility
    {
        public static bool Authorization(string auth, AuthSettings appsetting)
        {
            string authstring = $"{appsetting.Username}/{appsetting.Password}/{appsetting.SecretKey}";
            string hashString = GenerateSHA512String(authstring);

            return hashString == auth;
        }

        private static string GenerateSHA512String(string inputString)
        {
            SHA512 sha512 = SHA512Managed.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(inputString);
            byte[] hash = sha512.ComputeHash(bytes);
            return GetStringFromHash(hash);
        }

        private static string GetStringFromHash(byte[] hash)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2"));
            }
            return result.ToString();
        }

        public static bool FraudCheck(FundsTransferRequest request)
        {

            string r = RandomGeneratedStrings(request.cract, string.Empty, string.Empty,
                request.dract, request.trnamt.ToString(), string.Empty, string.Empty, request.trnrefno, request.trans_type);

            return r == request.guid;
        }

        public static string RandomGeneratedStrings(string creditAccount1, string creditAccount2, string creditAccount3,
                                             string debitAccount, string amount1, string amount2, string amount3,
                                             string referenceNo, int transType)
        {
            string result = "";
            try
            {
                creditAccount1 = creditAccount1.PadLeft(11, '0');
                creditAccount2 = creditAccount2.PadLeft(11, '0');
                creditAccount3 = creditAccount3.PadLeft(11, '0');

                debitAccount = debitAccount.PadLeft(11, '0');
                amount1 = decimal.Parse(amount1).ToString("F");
                string str1 = creditAccount1.Substring(0, 6);
                string str2 = referenceNo.Substring(0, 10);
                string str3 = debitAccount.Substring(debitAccount.Length - 3);
                string str4 = "010";
                string str5 = DateTime.Now.ToString("dd");
                string str6 = debitAccount.Substring(5, 3);
                string str7 = "0200";
                string str8 = amount1.Split('.')[0].PadLeft(10, '0');
                string str9 = creditAccount1.Substring(creditAccount1.Length - 5);
                string str10 = debitAccount.Substring(0, 5);
                string str11 = DateTime.Now.ToString("MMyyyy");
                string str12 = referenceNo.Substring(10, 5);
                string str13 = amount1.Split('.')[1];
                string str14 = referenceNo.Substring(15, 9);

                result = $"{str1}{str2}{str3}{str4}{str5}{str6}{str7}{str8}{str9}{str10}{str11}{str12}{str13}{str14}";
                switch (transType)
                {
                    case 1://payment

                        break;
                    case 2://cheque payment
                        amount2 = decimal.Parse(amount2).ToString("F");
                        amount3 = decimal.Parse(amount3).ToString("F");
                        string str15 = creditAccount2.Substring(0, 6);
                        string str16 = amount2.Split('.')[0].PadLeft(10, '0');
                        string str17 = creditAccount3.Substring(creditAccount3.Length - 6);
                        string str18 = amount3.Split('.')[0].PadLeft(10, '0');
                        string str19 = creditAccount2.Substring(creditAccount2.Length - 5);
                        string str20 = amount2.Split('.')[1];
                        string str21 = creditAccount3.Substring(0, 5);
                        string str22 = amount3.Split('.')[1];
                        result = $"{result}{str15}{str16}{str17}{str18}{str19}{str20}{str21}{str22}";
                        break;
                    case 3:
                        amount2 = decimal.Parse(amount2).ToString("F");
                        string str23 = creditAccount2.Substring(0, 6);
                        string str24 = amount2.Split('.')[0].PadLeft(10, '0');
                        string str25 = creditAccount2.Substring(creditAccount2.Length - 5);
                        string str26 = amount2.Split('.')[1];

                        result = $"{result}{str23}{str24}{str25}{str26}";
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return result;
        }

        public static TransLog GetTransLog(FundsTransferRequest request)
        {
            return new TransLog()
            {
                creditaccount = request.cract,
                debitaccount = request.dract,
                debit_branch = request.branch_code,
                post_amt = request.trnamt.ToString(),
                refno = request.trnrefno
            };
        }
    }
}
