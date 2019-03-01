using AccountOpening.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AccountOpening.Helpers
{
    public static class Utility
    {
        public static AccountOpeningResponse GetResponse_(ModelStateDictionary ModelState)
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
            return new AccountOpeningResponse()
            {
                status = false,
                message = errormsg
            };
        }
        public static AccountOpeningResponse2 GetResponse(ModelStateDictionary ModelState)
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

            return new AccountOpeningResponse2()
            {
                status = false,
                message = errormsg
            };
        }
        public static AccountOpeningResponse GetResponse(Exception ex)
        {
            Console.WriteLine(ex.ToString());
            List<string> errormsg = new List<string>();
            errormsg.Add(ex.Message);

            return new AccountOpeningResponse()
            {
                status = false,
                message = errormsg
            };
        }
        public static AccountOpeningResponse GetResponse(Exception ex, HttpStatusCode statuscode)
        {
            Console.WriteLine(ex.ToString());
            List<string> errormsg = new List<string>();
            errormsg.Add(ex.Message);

            return new AccountOpeningResponse()
            {
                status = false,
                message = errormsg
            };
        }
        public static AccountOpeningResponse GetResponse(string msg, HttpStatusCode statuscode)
        {
            Console.WriteLine(msg);
            List<string> errormsg = new List<string>();
            errormsg.Add(msg);

            return new AccountOpeningResponse()
            {
                status = false,
                message = errormsg
            };
        }

        public static UploadCustomer GetUploadCustomer(Customer c)
        {
            UploadCustomer u = new UploadCustomer();

            u.SOURCE_CODE = "CUST_UPD";
            u.MAINTENANCE_SEQ_NO = GetReferenceNo();// = helper.GetReferenceNo();
            u.CUSTOMER_NO = "AUTO";
            u.CUSTOMER_TYPE = c.CUSTOMER_TYPE;
            u.CUSTOMER_NAME1 = c.CUSTOMER_NAME;
            u.ADDRESS_LINE1 = c.D_ADDRESS1;
            u.ADDRESS_LINE3 = c.D_ADDRESS3;
            u.ADDRESS_LINE2 = c.D_ADDRESS2;
            u.ADDRESS_LINE4 = c.D_ADDRESS4;
            u.COUNTRY = c.COUNTRY;
            u.SHORT_NAME = c.SHORT_NAME;
            u.NATIONALITY = c.NATIONALITY;
            u.LANGUAGE = c.LANGUAGE;
            u.EXPOSURE_COUNTRY = c.COUNTRY;
            u.LOCAL_BRANCH = c.BRANCH_CODE;
            u.LIABILITY_NO = "AUTO";
            u.FROZEN = "N";
            u.DECEASED = "N";
            u.WHEREABOUTS_UNKNOWN = "N";
            u.CUSTOMER_CATEGORY = c.CUSTOMER_CATEGORY;
            u.FX_CUST_CLEAN_RISK_LIMIT = "0";
            u.OVERALL_LIMIT = "0";
            u.FX_CLEAN_RISK_LIMIT = "0";
            u.CREDIT_RATING = "";
            u.REVISION_DATE = DateTime.Now.ToString();
            u.LIMIT_CCY = "CDF";
            u.CAS_CUST = "N";
            u.CONVERSION_STATUS_FLAG = "U";
            u.SEC_CLEAN_RISK_LIMIT = "0";
            u.SEC_CUST_PSTL_RISK_LIMIT = "0";
            u.SEC_PSTL_RISK_LIMIT = "0";
            u.SWIFT_CODE = "";
            u.LIAB_BR = "";
            u.LIAB_NODE = "DRCONGDB";
            u.DEFAULT_MEDIA = "SWIFT";
            u.SHORT_NAME2 = "";
            u.SSN = "";
            u.ACTION_CODE = "NEW";
            u.UTILITY_PROVIDER = "N";
            u.MAILERS_REQUIRED = "N";
            u.AML_REQUIRED = "N";
            u.FT_ACCTING_AS_OF = "N";
            u.CUST_UNADVISED = "N";
            u.LIAB_UNADVISED = "N";
            u.CONSOL_TAX_CERT_REQD = "N";
            u.INDIVIDUAL_TAX_CERT_REQD = "N";
            u.UNIQUE_ID_NAME = c.UNIQUE_ID_NAME;
            u.UNIQUE_ID_VALUE = c.UNIQUE_ID_VALUE;
            u.FAX_NUMBER = c.FAX;
            u.SEC_CUST_CLEAN_RISK_LIMIT = "0";

            return u;
        }

        public static UploadPersonal GetUploadPersonal(Customer c)
        {
            UploadPersonal u = new UploadPersonal();

            u.MAINTENANCE_SEQ_NO = GetReferenceNo();
            u.CUSTOMER_PREFIX = c.CUSTOMER_PREFIX;
            u.FIRST_NAME = c.FIRST_NAME;
            u.MIDDLE_NAME = c.MIDDLE_NAME;
            u.LAST_NAME = c.LAST_NAME;
            u.DATE_OF_BIRTH = c.DATE_OF_BIRTH;
            u.MINOR = c.MINOR;
            u.SEX = c.SEX;
            u.D_ADDRESS1 = c.D_ADDRESS1;
            u.D_ADDRESS2 = c.D_ADDRESS2;
            u.D_ADDRESS3 = c.D_ADDRESS3;
            u.CONVERSION_STATUS_FLAG = "U";
            //u.MAINTENANCE_SEQ_NO = "";
            u.PASSPORT_NO = c.PASSPORT_NO;
            u.PPT_ISS_DATE = c.PPT_ISS_DATE;
            u.PPT_EXP_DATE = c.PPT_EXP_DATE;
            u.TELEPHONE = c.TELEPHONE;
            u.FAX = c.FAX;
            u.E_MAIL = c.E_MAIL;
            u.P_ADDRESS1 = c.P_ADDRESS1;
            u.P_ADDRESS3 = c.P_ADDRESS3;
            u.P_ADDRESS2 = c.P_ADDRESS2;
            u.D_COUNTRY = c.COUNTRY;
            u.P_COUNTRY = c.COUNTRY;

            return u;
        }
        public static UploadAccount GetAccountUpload(Customer c)
        {
            UploadAccount u = new UploadAccount();

            u.MAINTENANCE_SEQ_NO = c.MAINTENANCE_SEQ_NO; //=helper.GetReferenceNo();
            u.SOURCE_CODE = "CUSTACC_UPLOAD";
            u.BRANCH_CODE = c.BRANCH_CODE;
            u.CUST_AC_NO = "AUTO";
            u.AC_DESC = c.CUSTOMER_NAME;
            u.CUST_NO = c.CUSTOMER_NO;
            u.CCY = c.AMOUNTS_CCY;
            u.ACCOUNT_CLASS = c.ACCOUNT_CLASS;
            u.AC_STAT_NO_DR = "N";
            u.AC_STAT_NO_CR = "N";
            u.AC_STAT_BLOCK = "N";
            u.AC_STAT_STOP_PAY = "N";
            u.AC_STAT_DORMANT = "N";
            u.JOINT_AC_INDICATOR = "S";
            u.AC_OPEN_DATE = DateTime.Parse("13-JAN-2019"); //DateTime.Parse(DateTime.Now.ToString("dd-MMM-yyyy"));// DateTime.Parse("11-JAN-2019"); //DateTime.Now;//23-JUL-2018
            u.AC_STMT_DAY = 31;
            u.AC_STMT_CYCLE = "M";
            u.ACC_STMT_DAY2 = 31;
            u.AC_STMT_CYCLE2 = "M";
            u.ACC_STMT_TYPE2 = "D";
            u.CHEQUE_BOOK_FACILITY = "Y";
            u.ATM_FACILITY = "Y";
            u.PASSBOOK_FACILITY = "Y";
            u.AC_STMT_TYPE = "D";
            u.GEN_STMT_ONLY_ON_MVMT2 = "N";
            u.GEN_STMT_ONLY_ON_MVMT3 = "N";
            u.ACC_STMT_DAY3 = 31;
            u.AC_STMT_CYCLE3 = "M";
            u.ACC_STMT_TYPE3 = "D";
            u.DR_HO_LINE = c.DR_HO_LINE;
            u.CR_HO_LINE = c.CR_HO_LINE;
            u.CR_CB_LINE = c.CR_CB_LINE;
            u.DR_CB_LINE = c.DR_CB_LINE;
            u.DR_GL = c.DR_GL;
            u.CR_GL = c.CR_GL;
            u.ADDRESS1 = c.D_ADDRESS1;
            u.ADDRESS2 = c.D_ADDRESS2;
            u.ADDRESS3 = c.D_ADDRESS3;
            u.ADDRESS4 = c.D_ADDRESS4;
            u.ACC_STATUS = "NORM";
            u.ACTION_CODE = "";
            u.INHERIT_REPORTING = "Y";
            u.STATUS_SINCE = DateTime.Parse("13-JAN-2019"); // DateTime.Parse(DateTime.Now.ToString("dd-MMM-yyyy"));
            u.TYPE_OF_CHQ = "C";
            u.GEN_STMT_ONLY_ON_MVMT = "N";
            u.AC_STAT_DE_POST = "Y";
            u.DISPLAY_IBAN_IN_ADVICES = "N";
            u.REG_CC_AVAILABILITY = "N";
            u.MT210_REQD = "N";
            u.SWEEP_TYPE = "-1";
            u.DORMANT_PARAM = "B";
            u.POSITIVE_PAY_AC = "N";
            u.TRACK_RECEIVABLE = "N";
            u.NETTING_REQUIRED = "N";
            u.LODGEMENT_BOOK_FACILITY = "N";
            u.REFERRAL_REQUIRED = "N";
            u.EXCL_SAMEDAY_RVRTRNS_FM_STMT = "N";
            u.ALLOW_BACK_PERIOD_ENTRY = "Y";
            u.PROV_CCY_TYPE = "L";
            u.DEFER_RECON = "N";
            u.CONSOLIDATION_REQD = "N";
            u.FUNDING = "N";
            u.ALT_AC_NO = GetReferenceNo();
            u.AUTO_PROV_REQD = "N";

            return u;
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
