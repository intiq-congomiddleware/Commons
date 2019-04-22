using Commons.Entities;
using Dapper;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Options;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransactionStatus.Entities
{
    public class TransactionStatusRepository : ITransactionStatusRepository
    {
        private readonly AppSettings _appSettings;
        private IDataProtector _protector;

        public TransactionStatusRepository(IOptions<AppSettings> appSettings, IDataProtectionProvider provider)
        {
            _appSettings = appSettings.Value;
            _protector = provider.CreateProtector("treyryug");
        }

        public async Task<FundsTransferResponse> ValidateTransactionByRef(string transactionRef)
        {
            FundsTransferResponse response = new FundsTransferResponse();

            var oralConnect = new OracleConnection(_protector.Unprotect(_appSettings.FlexConnection));

            using (oralConnect)
            {
                string query = $@"SELECT * FROM {_appSettings.FlexSchema}.ACTB_DAILY_LOG WHERE AUTH_STAT = ‘A’ AND TRN_REF_NO = :transactionRef";

                var r = await oralConnect.QueryAsync<string>(query, transactionRef);

                response.message = r.FirstOrDefault();
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
