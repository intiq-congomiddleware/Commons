using Commons.Entities;
using Dapper;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Options;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebitFreeze.Entities
{
    public class DebitFreezeRepository : IDebitFreezeRepository
    {
        private readonly AppSettings _appSettings;
        private IDataProtector _protector;

        public DebitFreezeRepository(IOptions<AppSettings> appSettings, IDataProtectionProvider provider)
        {
            _appSettings = appSettings.Value;
            _protector = provider.CreateProtector("treyryug");
        }
        public async Task<Response> FreezeAccount(string accountNumber)
        {
            Response response = new Response();

            var oralConnect = new OracleConnection(_protector.Unprotect(_appSettings.FlexConnection));

            using (oralConnect)
            {
                string query = $@"UPDATE {_appSettings.FlexSchema}.STTM_CUST_ACCOUNT SET AC_STAT_NO_DR = 'Y' WHERE CUST_AC_NO = :accountNumber";

                var r = await oralConnect.QueryAsync<string>(query, new { accountNumber });

                response.message = r.FirstOrDefault();

                if (!string.IsNullOrWhiteSpace(response.message)) { response.status = true; }
            }

            return response;
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

