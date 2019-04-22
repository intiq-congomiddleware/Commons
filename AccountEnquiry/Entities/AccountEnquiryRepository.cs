using Commons.Entities;
using Microsoft.Extensions.Options;
using Oracle.ManagedDataAccess.Client;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;

namespace AccountEnquiry.Entities
{
    public class AccountEnquiryRepository : IAccountEnquiryRepository
    {
        private readonly AppSettings _appSettings;
        //private readonly IDataProtectionProvider _provider;
        private IDataProtector _protector;

        public AccountEnquiryRepository(IOptions<AppSettings> appSettings, IDataProtectionProvider provider)
        {
            _appSettings = appSettings.Value;
            _protector = provider.CreateProtector("treyryug");
        }

        public async Task<AccountEnquiryResponse> GetAccountEnquiryByAccountNumber(AccountEnquiryRequest request)
        {
            AccountEnquiryResponse ar = new AccountEnquiryResponse();

            var oralConnect = new OracleConnection(_protector.Unprotect(_appSettings.FlexConnection));
            using (oralConnect)
            {
                string query = $@"SELECT LPAD(A.BRANCH_CODE,3,0) COD_CC_BRN, A.CUST_AC_NO COD_ACCT_NO, A.AC_DESC COD_ACCT_TITLE,
                                  DECODE(C.ACCOUNT_CLASS,'222','S',C.ac_class_type) ACCOUNT_TYPE,
                                  A.CCY NAM_CCY_SHORT, A.AC_OPEN_DATE DAT_ACCT_OPEN, A.CUST_NO COD_CUST,A.AC_STAT_NO_DR, A.AC_STAT_NO_CR,
                                  A.AC_STAT_BLOCK, A.AC_STAT_STOP_PAY, A.AC_STAT_DORMANT, A.AC_STAT_FROZEN, A.ACCOUNT_CLASS COD_PROD,
                                  A.AC_STAT_DE_POST,C.description ACCOUNTDESC, BR.branch_name BRANCH,
                                  A.ACY_AVL_BAL BAL_AVAILABLE, Y.DATE_OF_BIRTH,NVL(Y.TELEPHONE, Y.FAX) CUSTOMER_PHONENUMBER,DECODE(B.CUSTOMER_TYPE,'C','CORPORATE','I','INDIVIDUAL') CUSTOMER_CATEGORY
                                  FROM {_appSettings.FlexSchema}.STTM_CUST_ACCOUNT A
                                  INNER JOIN {_appSettings.FlexSchema}.STTM_CUSTOMER B ON B.CUSTOMER_NO = A.CUST_NO
                                  LEFT OUTER JOIN {_appSettings.FlexSchema}.STTM_CUST_PERSONAL Y ON Y.CUSTOMER_NO = A.CUST_NO
                                  LEFT OUTER JOIN {_appSettings.FlexSchema}.MITB_CLASS_MAPPING D ON D.UNIT_REF_NO = A.CUST_AC_NO 
                                  LEFT OUTER JOIN {_appSettings.FlexSchema}.STTM_CUST_CORPORATE Z ON Z.CUSTOMER_NO=A.CUST_NO
                                  LEFT OUTER JOIN {_appSettings.FlexSchema}.STTM_ACCOUNT_CLASS C ON C.account_class = A.account_class 
                                  LEFT OUTER JOIN {_appSettings.FlexSchema}.STTM_BRANCH BR ON BR.branch_code = A.branch_code
                                  WHERE A.CUST_AC_NO in (:accountNumber)";

                var ars = await oralConnect.QueryAsync<AccountEnquiryResponse>(query, new { request.accountNumber });

                ar = ars.FirstOrDefault();
            }
            return ar;
        }

        public async Task<List<AccountEnquiryResponse>> GetAccountEnquiryByCustomerNumber(CustomerEnquiryRequest request)
        {
            List<AccountEnquiryResponse> ar = new List<AccountEnquiryResponse>();

            var oralConnect = new OracleConnection(_protector.Unprotect(_appSettings.FlexConnection));
            using (oralConnect)
            {
                string query = $@"SELECT LPAD(A.BRANCH_CODE,3,0) COD_CC_BRN, A.CUST_AC_NO COD_ACCT_NO, A.AC_DESC COD_ACCT_TITLE,
                                  DECODE(C.ACCOUNT_CLASS,'222','S',C.ac_class_type) ACCOUNT_TYPE,
                                  A.CCY NAM_CCY_SHORT, A.AC_OPEN_DATE DAT_ACCT_OPEN, A.CUST_NO COD_CUST,A.AC_STAT_NO_DR, A.AC_STAT_NO_CR,
                                  A.AC_STAT_BLOCK, A.AC_STAT_STOP_PAY, A.AC_STAT_DORMANT, A.AC_STAT_FROZEN, A.ACCOUNT_CLASS COD_PROD,
                                  A.AC_STAT_DE_POST,C.description ACCOUNTDESC, BR.branch_name BRANCH,
                                  A.ACY_AVL_BAL BAL_AVAILABLE, Y.DATE_OF_BIRTH,NVL(Y.TELEPHONE, Y.FAX) CUSTOMER_PHONENUMBER,DECODE(B.CUSTOMER_TYPE,'C','CORPORATE','I','INDIVIDUAL') CUSTOMER_CATEGORY
                                  FROM {_appSettings.FlexSchema}.STTM_CUST_ACCOUNT A
                                  INNER JOIN {_appSettings.FlexSchema}.STTM_CUSTOMER B ON B.CUSTOMER_NO = A.CUST_NO
                                  LEFT OUTER JOIN {_appSettings.FlexSchema}.STTM_CUST_PERSONAL Y ON Y.CUSTOMER_NO = A.CUST_NO
                                  LEFT OUTER JOIN {_appSettings.FlexSchema}.MITB_CLASS_MAPPING D ON D.UNIT_REF_NO = A.CUST_AC_NO 
                                  LEFT OUTER JOIN {_appSettings.FlexSchema}.STTM_CUST_CORPORATE Z ON Z.CUSTOMER_NO=A.CUST_NO
                                  LEFT OUTER JOIN {_appSettings.FlexSchema}.STTM_ACCOUNT_CLASS C ON C.account_class = A.account_class 
                                  LEFT OUTER JOIN {_appSettings.FlexSchema}.STTM_BRANCH BR ON BR.branch_code = A.branch_code
                                  WHERE B.CUSTOMER_NO in (:customerNumber) ";

                var ars = await oralConnect.QueryAsync<AccountEnquiryResponse>(query, new { request.customerNumber });

                ar = ars.ToList();
            }
            return ar;
        }

        public async Task<List<AccountEnquiryResponse>> GetAccountEnquiryByPhoneNumber(PhoneEnquiryRequest request)
        {
            List<AccountEnquiryResponse> ar = new List<AccountEnquiryResponse>();

            var oralConnect = new OracleConnection(_protector.Unprotect(_appSettings.FlexConnection));
            using (oralConnect)
            {
                string query = $@"SELECT LPAD(A.BRANCH_CODE,3,0) COD_CC_BRN, A.CUST_AC_NO COD_ACCT_NO, A.AC_DESC COD_ACCT_TITLE,
                                  DECODE(C.ACCOUNT_CLASS,'222','S',C.ac_class_type) ACCOUNT_TYPE,
                                  A.CCY NAM_CCY_SHORT, A.AC_OPEN_DATE DAT_ACCT_OPEN, A.CUST_NO COD_CUST,A.AC_STAT_NO_DR, A.AC_STAT_NO_CR,
                                  A.AC_STAT_BLOCK, A.AC_STAT_STOP_PAY, A.AC_STAT_DORMANT, A.AC_STAT_FROZEN, A.ACCOUNT_CLASS COD_PROD,
                                  A.AC_STAT_DE_POST,C.description ACCOUNTDESC, BR.branch_name BRANCH,
                                  A.ACY_AVL_BAL BAL_AVAILABLE, Y.DATE_OF_BIRTH,NVL(Y.TELEPHONE, Y.FAX) CUSTOMER_PHONENUMBER,DECODE(B.CUSTOMER_TYPE,'C','CORPORATE','I','INDIVIDUAL') CUSTOMER_CATEGORY
                                  FROM {_appSettings.FlexSchema}.STTM_CUST_ACCOUNT A
                                  INNER JOIN {_appSettings.FlexSchema}.STTM_CUSTOMER B ON B.CUSTOMER_NO = A.CUST_NO
                                  INNER JOIN {_appSettings.FlexSchema}.STTM_CUST_PERSONAL Y ON Y.CUSTOMER_NO = A.CUST_NO
                                  LEFT OUTER JOIN {_appSettings.FlexSchema}.MITB_CLASS_MAPPING D ON D.UNIT_REF_NO = A.CUST_AC_NO 
                                  LEFT OUTER JOIN {_appSettings.FlexSchema}.STTM_CUST_CORPORATE Z ON Z.CUSTOMER_NO=A.CUST_NO
                                  LEFT OUTER JOIN {_appSettings.FlexSchema}.STTM_ACCOUNT_CLASS C ON C.account_class = A.account_class 
                                  LEFT OUTER JOIN {_appSettings.FlexSchema}.STTM_BRANCH BR ON BR.branch_code = A.branch_code
                                  WHERE (Y.TELEPHONE in (:phoneNumber) OR Y.FAX in (:phoneNumber)) ";

                var ars = await oralConnect.QueryAsync<AccountEnquiryResponse>(query, new { request.phoneNumber });

                ar = ars.ToList();
            }
            return ar;
        }

        public string EncData(string value)
        {
            string output = string.Empty;
            try
            {
                output = _protector.Protect(value);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return output;
        }
    }
}
