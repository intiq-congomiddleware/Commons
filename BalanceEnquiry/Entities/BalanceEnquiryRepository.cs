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
                string query = $@"select * from {_appSettings.FlexSchema}.tbl_statement_summary where run_userid = :userId and accountno = :accountNumber";

                var b = await oralConnect.QueryAsync<BalanceEnquiryResponse>(query, new
                {
                    request.accountNumber,
                    request.userId
                });

                br = b.FirstOrDefault();
            }
            return br;
        }
    }
}


