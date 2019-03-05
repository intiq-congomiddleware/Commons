using Dapper;
using Microsoft.Extensions.Options;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Commons.Entities;

namespace BalanceEnquiry.Entities
{
    public class BalanceEnquiryRepository : IBalanceEnquiryRepository
    {
        private readonly AppSettings _appSettings;

        public BalanceEnquiryRepository(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public async Task<BalanceEnquiryResponse> GetBalanceEnquiry(BalanceEnquiryRequest request)
        {
            BalanceEnquiryResponse br = new BalanceEnquiryResponse();

            var oralConnect = new OracleConnection(_appSettings.FlexConnection);
            using (oralConnect)
            {
                //Confirm if ther is phone number on this table
               string query = $@"select ACCOUNT_CLASS COD_PROD, CUST_AC_NO COD_ACCT_NO, acy_curr_balance,
                              tod_limit+acy_curr_balance as BAL_AVAILABLE, BRANCH_CODE COD_CC_BRN
                              from {_appSettings.FlexSchema}.sttm_cust_account where cust_ac_no = :accountNumber and record_stat = 'O' and tod_limit> 0
                              and trunc(tod_limit_end_date) >= trunc(sysdate) ";

                var brs = await oralConnect.QueryAsync<BalanceEnquiryResponse>(query, new { request.accountNumber });

                if (brs == null)
                {
                    string new_query = $@" SELECT ACCOUNT_CLASS COD_PROD, CUST_AC_NO COD_ACCT_NO,
                                  nvl({_appSettings.FlexSchema}.GET_AC_CUST_DLY_CLOSING_BAL(:accountNumber, sysdate), 0)  BAL_AVAILABLE,
                                  BRANCH_CODE COD_CC_BRN
                                  FROM {_appSettings.FlexSchema}.STTM_CUST_ACCOUNT  WHERE CUST_AC_NO = :accountNumber";

                    brs = await oralConnect.QueryAsync<BalanceEnquiryResponse>(new_query, new { request.accountNumber });
                }

                br = brs.FirstOrDefault();
            }
            return br;
        }
    }
}


