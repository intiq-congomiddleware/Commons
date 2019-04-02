using Dapper;
using Microsoft.Extensions.Options;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Commons.Entities;
using Microsoft.AspNetCore.DataProtection;

namespace BalanceEnquiry.Entities
{
    public class BalanceEnquiryRepository : IBalanceEnquiryRepository
    {
        private readonly AppSettings _appSettings;
        //private readonly IDataProtectionProvider _provider;
        private IDataProtector _protector;

        public BalanceEnquiryRepository(IOptions<AppSettings> appSettings, IDataProtectionProvider provider)
        {
            _appSettings = appSettings.Value;
            _protector = provider.CreateProtector("treyryug");
        }

        public async Task<BalanceEnquiryResponse> GetBalanceEnquiry(BalanceEnquiryRequest request)
        {
            //EncData();
            BalanceEnquiryResponse br = new BalanceEnquiryResponse();

            var oralConnect = new OracleConnection(_protector.Unprotect(_appSettings.FlexConnection));
            using (oralConnect)
            {
                //Confirm if ther is phone number on this table
               string query = $@"select AC_DESC CUST_NAME, CCY CURRENCY, DECODE(ACCOUNT_TYPE, 'U', 'CURRENT ACCOUNT', 'S', 'SAVINGS ACCOUNT', 'NULL') ACCT_TYPE,
                              ACCOUNT_CLASS COD_PROD, CUST_AC_NO COD_ACCT_NO, acy_curr_balance UNCLEARD_BAL,
                              tod_limit+acy_curr_balance as BAL_AVAILABLE, BRANCH_CODE COD_CC_BRN
                              from {_appSettings.FlexSchema}.sttm_cust_account where cust_ac_no = :accountNumber and record_stat = 'O' and tod_limit> 0
                              and trunc(tod_limit_end_date) >= trunc(sysdate)";

                var brs = await oralConnect.QueryAsync<BalanceEnquiryResponse>(query, new { request.accountNumber });

                if (brs.Count() <= 0)
                {
                    string new_query = $@"SELECT AC_DESC CUST_NAME, CCY CURRENCY, DECODE(ACCOUNT_TYPE, 'U', 'CURRENT ACCOUNT', 'S', 'SAVINGS ACCOUNT', 'NULL') ACCT_TYPE,
                                  ACCOUNT_CLASS COD_PROD, CUST_AC_NO COD_ACCT_NO,  acy_curr_balance UNCLEARD_BAL,
                                  nvl({_appSettings.FlexSchema}.GET_AC_CUST_DLY_CLOSING_BAL(:accountNumber, sysdate), 0)  BAL_AVAILABLE,
                                  BRANCH_CODE COD_CC_BRN FROM {_appSettings.FlexSchema}.STTM_CUST_ACCOUNT  WHERE CUST_AC_NO = :accountNumber";

                    brs = await oralConnect.QueryAsync<BalanceEnquiryResponse>(new_query, new { request.accountNumber });
                }

                br = brs.FirstOrDefault();
            }
            return br;
        }        
    }
}


