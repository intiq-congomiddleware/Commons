﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Commons.Entities;
using Dapper;
using Microsoft.Extensions.Options;
using Oracle.ManagedDataAccess.Client;

namespace FundsTransfer.Entities
{
    public class FundsTransferRepository : IFundsTransferRepository
    {
        private readonly AppSettings _appSettings;

        public FundsTransferRepository(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public async Task<bool> ValidateTransactionByRef(TransLog transLog)
        {
            bool response = false;

            var oralConnect = new OracleConnection(_appSettings.TMEConnection);

            using (oralConnect)
            {
                string query = $@"select *  from {_appSettings.TMESchema}.tme_postedtxn where debitaccount=:debitaccount
                                    and creditaccount=:creditaccount and post_amt=:post_amt and id=:refno 
                                    and entry_branch=:debit_branch and responsecode='01'";

                var r = await oralConnect.QueryAsync<string>(query, transLog);

                response = r != null && r.Count() > 0;
            }

            return response;
        }

        public async Task<FundsTransferResponse> ExecuteTransaction(FundsTransferRequest request, string sproc)
        {
            FundsTransferResponse resp = new FundsTransferResponse();

            string storeProcedure = $"{_appSettings.FlexSchema}.{sproc}";

            var oralConnect = new OracleConnection(_appSettings.FlexConnection);

            var param = new DynamicParameters();
            param.Add("dract", request.dract.Trim());
            if (!request.with_charges) param.Add("cract", request.cract?.Trim());
            if (request.with_charges) param.Add("cract1", request.cract1?.Trim());
            if (request.with_charges) param.Add("cract2", request.cract2?.Trim());
            param.Add("trnamt", request.trnamt.Trim());
            if (request.with_charges) param.Add("trnamt1", request.trnamt1.Trim());
            param.Add("trnrefno", request.trnrefno.Trim());
            param.Add("l_acs_ccy", request.l_acs_ccy.Trim());
            param.Add("txnnarra", request.txnnarra);
            //if (request.with_charges) param.Add("prate", request.prate?.Trim());
            param.Add("product", request.product.Trim());
            param.Add("instr_code", request.instr_code.Trim());
            param.Add("branch_code", request.branch_code.Trim());
            param.Add("user_name", request.user_name);
            param.Add("response_code", direction: ParameterDirection.Output, size: 100);
            param.Add("response_msg", direction: ParameterDirection.Output, size: 300);
            param.Add("actualtrnamt", direction: ParameterDirection.Output, size: 300);
            param.Add("rate", direction: ParameterDirection.Output, size: 300);

            using (oralConnect)
            {
                oralConnect.Open();
                await oralConnect.ExecuteAsync(storeProcedure, param, commandType: CommandType.StoredProcedure);

                resp.status = param.Get<string>("response_code");
                resp.message = param.Get<string>("response_msg");
                resp.actualtrnamt = param.Get<string>("actualtrnamt");
                resp.rate = param.Get<string>("rate");
                resp.trnrefno = request.trnrefno;
                resp.status = resp.status.Trim();
            }

            return resp;
        }

        public async Task<bool> UpdateTransactionResponse(FundsTransferResponse resp)
        {
            bool response = false;

            var oralConnect = new OracleConnection(_appSettings.FlexConnection);
            using (oralConnect)
            {
                string query = $@" Update {_appSettings.FlexSchema}.tme_postedtxn set transactionreference =:trnrefno, 
                                    transactionresponsecode =:response_code,transactionMessage=:response_msg, 
                                    actualtransactionamount= :actualtrnamt,transactionrate=:rate
                                    where  id =:id ";

                var r = await oralConnect.ExecuteAsync(query, resp);
                response = r > 0;
            }

            return response;
        }

        public async Task<bool> IsOwnAccount(FundsTransferRequest request)
        {
            bool response = false;

            var oralConnect = new OracleConnection(_appSettings.TMEConnection);
            try
            {
                using (oralConnect)
                {
                    string query = $@"SELECT A.* FROM FCCUAT.STTM_CUST_ACCOUNT A INNER JOIN FCCUAT.STTM_CUST_ACCOUNT B ON A.CUST_NO = B.CUST_NO
                                WHERE A.CUST_AC_NO = :dract AND B.CUST_AC_NO = :cract";

                    var r = await oralConnect.QueryAsync<string>(query, new { request.dract, request.cract });

                    response = r != null && r.Count() > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }

            return response;
        }
    }
}
