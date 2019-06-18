using Commons.Entities;
using Dapper;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Options;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StatementGeneration.Entities
{
    public class StatementRepository : IStatementRepository
    {
        private readonly AppSettings _appSettings;
        private IDataProtector _protector;

        public StatementRepository(IOptions<AppSettings> appSettings, IDataProtectionProvider provider)
        {
            _appSettings = appSettings.Value;
            _protector = provider.CreateProtector("treyryug");
        }
        public async Task<int> GenerateStatement(StatementRequest request)
        {
            //List<StatementResponse> response = new List<StatementResponse>();
            int response = -1;

            //StatementRequestDTO req = new StatementRequestDTO()
            //{
            //    acct = request.accountNumber,
            //    end_dt = request.endDate.ToString("dd-MMM-yyyy"),
            //    start_dt = request.startDate.ToString("dd-MMM-yyyy"),
            //    runUSERID = request.userId
            //};

            //var parameters = new OracleDynamicParameters();

            //var param = new DynamicParameters();
            //param.Add("runUSERID", request.userId.Trim());
            //param.Add("acct", request.accountNumber.Trim());
            //param.Add("start_dt", request.startDate.ToString("dd-MMM-yyyy"));
            //param.Add("end_dt", request.endDate.ToString("dd-MMM-yyyy"));           

            var oralConnect = new OracleConnection(_protector.Unprotect(_appSettings.FlexConnection));

            using (oralConnect)
            {                
                string query = $@"SELECT {_appSettings.FlexSchema}.FN_STATEMENT_ENQ(:userId, :accountNumber, :startDate, :endDate) RETURN_VALUE FROM DUAL";

                var r = await oralConnect.ExecuteScalarAsync<int>(query, new { request.userId, request.accountNumber, request.startDate, request.endDate});

                response = r;
            }

            return response;
        }

        public async Task<List<StatementResponseDTO>> FilterStatement(StatementRequest request)
        {
            List<StatementResponseDTO> response = new List<StatementResponseDTO>();

            var oralConnect = new OracleConnection(_protector.Unprotect(_appSettings.FlexConnection));

            using (oralConnect)
            {
                string query = $@"SELECT A.RUN_USERID, A.ACCT_NO, A.TXN_DAT, A.VAL_DT, A.TXN_BRN, A.COD_USER_ID, A.TXN_DESC, A.REF_NO, A.DR_AMT, A.CR_AMT, A.RUNNING_BAL, A.DAT_POST, A.TRANSACTION_REFERENCE, A.TRAN_MNEMONIC, A.TRANS_SEQNO, 
                                  A.RUNDATE,B.STARTDATE, B.ENDDATE, B.TOTALDR, B.TOTALCR, B.DRCOUNT, B.CRCOUNT, B.CURRENCY, B.CUSTOMERNAME, B.OPENINGBALANCE, B.CLOSINGBALANCE, B.ADDRESS, E.BRANCH_CODE, E.ACCOUNT_CLASS, E.AC_STAT_NO_DR, E.AC_STAT_NO_CR,
                                  E.AC_STAT_BLOCK, E.AC_STAT_STOP_PAY, E.AC_STAT_DORMANT, E.AC_STAT_FROZEN, E.AC_STAT_DE_POST, E.ACY_AVL_BAL
                                  FROM {_appSettings.FlexSchema}.TBL_STATEMENT_SUMMARY B 
                                  INNER JOIN {_appSettings.FlexSchema}.TBL_STATEMENT_DETAILS A ON B.ACCOUNTNO = A.ACCT_NO  AND A.RUN_USERID = B.RUN_USERID
                                  INNER JOIN {_appSettings.FlexSchema}.STTM_CUST_ACCOUNT E ON E.CUST_AC_NO = B.ACCOUNTNO
                                  WHERE B.RUN_USERID = :userId AND B.ACCOUNTNO = :accountNumber ORDER BY TRANS_SEQNO";

                var r = await oralConnect.QueryAsync<StatementResponseDTO>(query, new { request.userId, request.accountNumber });

                response = r.ToList();
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
