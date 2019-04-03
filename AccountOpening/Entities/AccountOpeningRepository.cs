using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Commons.Entities;
using Microsoft.Extensions.Options;
using Oracle.ManagedDataAccess.Client;
using Microsoft.AspNetCore.DataProtection;

namespace AccountOpening.Entities
{
    public class AccountOpeningRepository : IAccountOpeningRepository
    {
        private readonly AppSettings _appSettings;
        //private readonly IDataProtectionProvider _provider;
        private IDataProtector _protector;

        public AccountOpeningRepository(IOptions<AppSettings> appSettings, IDataProtectionProvider provider)
        {
            _appSettings = appSettings.Value;
            _protector = provider.CreateProtector("treyryug");
        }

        public async Task<bool> AddCustomer(Personal p, Customer uc)
        {
            int rowAffected = 0;

            using (IDbConnection oralConnect = new OracleConnection(_protector.Unprotect(_appSettings.FlexConnection)))
            {
                oralConnect.Open();
                IDbTransaction transaction = oralConnect.BeginTransaction(IsolationLevel.ReadCommitted);
                string queryCustomer = $@" INSERT INTO {_appSettings.FlexSchema}.STTM_UPLOAD_CUSTOMER(SOURCE_CODE,MAINTENANCE_SEQ_NO,CUSTOMER_NO,CUSTOMER_TYPE,
                                                    CUSTOMER_NAME1,ADDRESS_LINE1, ADDRESS_LINE3,ADDRESS_LINE2,ADDRESS_LINE4,COUNTRY,SHORT_NAME,NATIONALITY,LANGUAGE,
                                                    EXPOSURE_COUNTRY,LOCAL_BRANCH,LIABILITY_NO,UNIQUE_ID_NAME,UNIQUE_ID_VALUE,FROZEN,DECEASED,WHEREABOUTS_UNKNOWN,
                                                    CUSTOMER_CATEGORY,FX_CUST_CLEAN_RISK_LIMIT,OVERALL_LIMIT,FX_CLEAN_RISK_LIMIT,CREDIT_RATING,LIMIT_CCY,CAS_CUST,
                                                    CONVERSION_STATUS_FLAG, SEC_CUST_CLEAN_RISK_LIMIT,SEC_CLEAN_RISK_LIMIT,SEC_CUST_PSTL_RISK_LIMIT,
                                                    SEC_PSTL_RISK_LIMIT,SWIFT_CODE,LIAB_BR,LIAB_NODE,DEFAULT_MEDIA,LOC_CODE,SHORT_NAME2,SSN,ACTION_CODE,
                                                    UTILITY_PROVIDER, MAILERS_REQUIRED,AML_REQUIRED,FT_ACCTING_AS_OF,LIAB_UNADVISED,
                                                    CONSOL_TAX_CERT_REQD,INDIVIDUAL_TAX_CERT_REQD,FAX_NUMBER)
                                                        VALUES(:SOURCE_CODE,:MAINTENANCE_SEQ_NO,:CUSTOMER_NO,:CUSTOMER_TYPE,:CUSTOMER_NAME1,:ADDRESS_LINE1,
                                                        :ADDRESS_LINE3,:ADDRESS_LINE2,:ADDRESS_LINE4,:COUNTRY,:SHORT_NAME,:NATIONALITY,:LANGUAGE,:EXPOSURE_COUNTRY,:LOCAL_BRANCH,:LIABILITY_NO,:UNIQUE_ID_NAME,:UNIQUE_ID_VALUE,:FROZEN,
                                                        :DECEASED,:WHEREABOUTS_UNKNOWN,:CUSTOMER_CATEGORY,:FX_CUST_CLEAN_RISK_LIMIT,
                                                        :OVERALL_LIMIT,:FX_CLEAN_RISK_LIMIT,:CREDIT_RATING,:LIMIT_CCY,:CAS_CUST,:CONVERSION_STATUS_FLAG,:SEC_CUST_CLEAN_RISK_LIMIT,:SEC_CLEAN_RISK_LIMIT,
                                                        :SEC_CUST_PSTL_RISK_LIMIT,:SEC_PSTL_RISK_LIMIT,:SWIFT_CODE,:LIAB_BR,:LIAB_NODE,:DEFAULT_MEDIA,
                                                        :LOC_CODE,:SHORT_NAME2,:SSN,:ACTION_CODE,:UTILITY_PROVIDER,:MAILERS_REQUIRED,
                                                        :AML_REQUIRED,:FT_ACCTING_AS_OF,:LIAB_UNADVISED,:CONSOL_TAX_CERT_REQD,:INDIVIDUAL_TAX_CERT_REQD,:FAX_NUMBER)";

                string queryPersonal = $@" INSERT INTO {_appSettings.FlexSchema}.STTM_UPLOAD_CUST_PERSONAL 
                                                        (CUSTOMER_PREFIX,FIRST_NAME,MIDDLE_NAME,LAST_NAME,DATE_OF_BIRTH,LEGAL_GUARDIAN,MINOR,SEX,P_NATIONAL_ID,
                                                        PASSPORT_NO,PPT_ISS_DATE,PPT_EXP_DATE,D_ADDRESS1,D_ADDRESS2,D_ADDRESS3,TELEPHONE,FAX,E_MAIL,P_ADDRESS1,
                                                        P_ADDRESS3,P_ADDRESS2,CUSTOMER_NO,D_COUNTRY,P_COUNTRY,MAINTENANCE_SEQ_NO)
                                                        VALUES(:CUSTOMER_PREFIX,:FIRST_NAME,:MIDDLE_NAME,:LAST_NAME,:DATE_OF_BIRTH,:LEGAL_GUARDIAN,:MINOR,:SEX,
                                                        :NATIONAL_ID,:PASSPORT_NO,:PPT_ISS_DATE,:PPT_EXP_DATE,:D_ADDRESS1,:D_ADDRESS2,:D_ADDRESS3,:TELEPHONE,
                                                        :FAX,:E_MAIL,:P_ADDRESS1,:P_ADDRESS3,:P_ADDRESS2,:CUSTOMER_NO,:D_COUNTRY,:P_COUNTRY,:MAINTENANCE_SEQ_NO)";

                rowAffected =  await oralConnect.ExecuteAsync(queryCustomer, uc, transaction);
                rowAffected += await oralConnect.ExecuteAsync(queryPersonal, p, transaction);

                transaction.Commit();
            }

            return (rowAffected > 0);
        }

        public async Task<bool> ExecuteNewCustomer(ExecuteCustomer executeCustomer)
        {
            var oralConnect = new OracleConnection(_protector.Unprotect(_appSettings.FlexConnection));
            string query = $@"DECLARE
                                      l_source_code           {_appSettings.FlexSchema}.cotms_source.source_code%TYPE := :SourceCode  ;
                                      l_branch_code           {_appSettings.FlexSchema}.sttm_branch.branch_code%TYPE  := :BranchCode  ;
                                      l_upload_seq_no        VARCHAR2(16);
                                      l_host_name          {_appSettings.FlexSchema}.sttms_branch_node.node%TYPE := :BranchNode;
                                      l_err_codes        VARCHAR2(1000);
                                      l_err_params        VARCHAR2(1000);
                                      l_rec_list              varchar2(1000);

                                    BEGIN

                                         {_appSettings.FlexSchema}.GLOBAL.PR_INIT(l_branch_code ,:UserId);
                                      IF NOT {_appSettings.FlexSchema}.stpks_cust_upload.fn_stupload
                                                              (    l_source_code
                                                              ,    l_branch_code
                                                              ,    l_upload_seq_no
                                                              ,    l_host_name
                                                              ,    l_err_codes
                                                              ,    l_err_params
                                                              ,    l_rec_list
                                                              )
                                      THEN    dbms_output.put_line ('stpkss_upload_customer...returned FALSE...'||l_err_codes||'..'||l_err_params);
 
                                      ELSE    dbms_output.put_line ('stpkss_upload_customer...returned TRUE...Happy');

                                      END IF;

                                    END;";

            var r = await oralConnect.ExecuteAsync(query, executeCustomer);

            return true;
        }

        public async Task<bool> CustomerExist(string customerNo)
        {
            //EncData();
            IEnumerable<CustomerNo> customer = new List<CustomerNo>();
            var oralConnect = new OracleConnection(_protector.Unprotect(_appSettings.FlexConnection));
            try
            {
                string query = $@"select * from {_appSettings.FlexSchema}.sttm_customer where customer_no=:customerNo AND record_stat='O'";
                using (oralConnect)
                {
                    customer = await oralConnect.QueryAsync<CustomerNo>(query, new { customerNo });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }  
            
            return (customer.Count() > 0);
        }       

        public async Task<bool> AddAccount(Account a)
        {
            int r = 0;
            var oralConnect = new OracleConnection(_protector.Unprotect(_appSettings.FlexConnection));
            using (oralConnect)
            {
                string queryAccount = $@"INSERT INTO {_appSettings.FlexSchema}.STTB_UPLOAD_CUST_ACCOUNT (MAINTENANCE_SEQ_NO,SOURCE_CODE,BRANCH_CODE,CUST_AC_NO,AC_DESC,CUST_NO,CCY,
                                                ACCOUNT_CLASS,AC_STAT_NO_DR,AC_STAT_NO_CR,AC_STAT_BLOCK,AC_STAT_STOP_PAY,AC_STAT_DORMANT,
                                                JOINT_AC_INDICATOR,AC_OPEN_DATE,AC_STMT_DAY,AC_STMT_CYCLE,ALT_AC_NO,CHEQUE_BOOK_FACILITY,
                                                ATM_FACILITY,PASSBOOK_FACILITY,AC_STMT_TYPE,DR_HO_LINE,CR_HO_LINE,CR_CB_LINE,DR_CB_LINE,                                           
                                                DR_GL,CR_GL,ACC_STMT_TYPE2,ACC_STMT_DAY2,
                                                AC_STMT_CYCLE2,
                                                GEN_STMT_ONLY_ON_MVMT2,ACC_STMT_TYPE3,ACC_STMT_DAY3,AC_STMT_CYCLE3,
                                                GEN_STMT_ONLY_ON_MVMT3,
                                                ADDRESS1,ADDRESS2,ADDRESS3,ADDRESS4,TYPE_OF_CHQ,
                                                GEN_STMT_ONLY_ON_MVMT,AC_STAT_DE_POST,DISPLAY_IBAN_IN_ADVICES,
                                                REG_CC_AVAILABILITY,MT210_REQD,SWEEP_TYPE,
                                                DORMANT_PARAM,POSITIVE_PAY_AC,TRACK_RECEIVABLE,
                                                NETTING_REQUIRED,LODGEMENT_BOOK_FACILITY,REFERRAL_REQUIRED,
                                                ACC_STATUS,STATUS_SINCE,INHERIT_REPORTING,
                                                EXCL_SAMEDAY_RVRTRNS_FM_STMT,ALLOW_BACK_PERIOD_ENTRY,
                                                PROV_CCY_TYPE,DEFER_RECON,CONSOLIDATION_REQD,
                                                ACTION_CODE,FUNDING,AUTO_PROV_REQD)
                                            VALUES(:MAINTENANCE_SEQ_NO,:SOURCE_CODE,:BRANCH_CODE,:CUST_AC_NO,:AC_DESC,:CUST_NO,:CCY,
                                                :ACCOUNT_CLASS,:AC_STAT_NO_DR,:AC_STAT_NO_CR,:AC_STAT_BLOCK,:AC_STAT_STOP_PAY,:AC_STAT_DORMANT,                                                
                                                :JOINT_AC_INDICATOR,:AC_OPEN_DATE,:AC_STMT_DAY,:AC_STMT_CYCLE,:ALT_AC_NO,:CHEQUE_BOOK_FACILITY,                                               
                                                :ATM_FACILITY,:PASSBOOK_FACILITY,:AC_STMT_TYPE,:DR_HO_LINE,:CR_HO_LINE,:CR_CB_LINE,:DR_CB_LINE,                                            
                                               :DR_GL,:CR_GL,:ACC_STMT_TYPE2,:ACC_STMT_DAY2,
                                                :AC_STMT_CYCLE2,
                                                :GEN_STMT_ONLY_ON_MVMT2,:ACC_STMT_TYPE3,:ACC_STMT_DAY3,:AC_STMT_CYCLE3,
                                                :GEN_STMT_ONLY_ON_MVMT3,
                                                :ADDRESS1,:ADDRESS2,:ADDRESS3,:ADDRESS4,:TYPE_OF_CHQ,
                                                :GEN_STMT_ONLY_ON_MVMT,:AC_STAT_DE_POST,:DISPLAY_IBAN_IN_ADVICES,
                                                :REG_CC_AVAILABILITY,:MT210_REQD,:SWEEP_TYPE,
                                                :DORMANT_PARAM,:POSITIVE_PAY_AC,:TRACK_RECEIVABLE,
                                                :NETTING_REQUIRED,:LODGEMENT_BOOK_FACILITY,:REFERRAL_REQUIRED,
                                                :ACC_STATUS,:STATUS_SINCE,:INHERIT_REPORTING,
                                                :EXCL_SAMEDAY_RVRTRNS_FM_STMT,:ALLOW_BACK_PERIOD_ENTRY,
                                                :PROV_CCY_TYPE,:DEFER_RECON,:CONSOLIDATION_REQD,
                                                :ACTION_CODE,:FUNDING,:AUTO_PROV_REQD)";

                oralConnect.Open();

                r = await oralConnect.ExecuteAsync(queryAccount, a);
            }

            return (r > 0);
        }

        public async Task<bool> ExecuteNewAccount(ExecuteCustomer executeCustomer)
        {
            var oralConnect = new OracleConnection(_protector.Unprotect(_appSettings.FlexConnection));
            string query = $@"DECLARE
                                   l_source_code           {_appSettings.FlexSchema}.cotms_source.source_code%TYPE := :SourceCode ;
                                   l_branch_code           {_appSettings.FlexSchema}.sttm_branch.branch_code%TYPE  := :BranchCode ;
                                   l_upload_seq_no         VARCHAR2(16);
                                   l_err_codes             VARCHAR2(1000);
                                   l_err_params            VARCHAR2(1000);

                                BEGIN

                                   {_appSettings.FlexSchema}.GLOBAL.PR_INIT(l_branch_code ,:UserId);
                                   IF NOT {_appSettings.FlexSchema}.stpkss_upload_cust_account.fn_stupload
                                                           (   l_source_code
                                                           ,   l_branch_code
                                                           ,   l_upload_seq_no
                                                           ,   l_err_codes
                                                           ,   l_err_params
                                                           )
                                   THEN    dbms_output.put_line ('stpkss_upload_cust_account...returned FALSE...'||l_err_codes||'..'||l_err_params);
   
                                   ELSE    dbms_output.put_line ('stpkss_upload_cust_account...returned TRUE...'||l_upload_seq_no);

                                   END IF;

                                END;";

            var r = await oralConnect.ExecuteAsync(query, executeCustomer);

            return true;
        }

        public async Task<AccountOpeningRequest> GetCustomer(string seq_num, string acct_class)
        {
            IEnumerable<AccountOpeningRequest> customer = new List<AccountOpeningRequest>();
            var oralConnect = new OracleConnection(_protector.Unprotect(_appSettings.FlexConnection));
            try
            {
                string query = $@"select a.*, c.account_class,a.customer_name1 customer_name,
                                  a.address_line1 D_address1,a.address_line2 D_address2,a.address_line3 D_address3,a.address_line4 D_address4,
                                  c.dr_gl,c.cr_gl,c.dr_cb_line,c.cr_cb_line,c.dr_ho_line,c.cr_ho_line,A.LOCAL_BRANCH 
                                  from {_appSettings.FlexSchema}.STTM_UPLOAD_CUSTOMER a
                                  left join {_appSettings.FlexSchema}.sttm_account_class_status c on c.account_class = :acct_class
                                  where a.MAINTENANCE_SEQ_NO = :seq_num AND c.status = 'NORM'";

                using (oralConnect)
                {
                    customer = await oralConnect.QueryAsync<AccountOpeningRequest>(query, new { seq_num, acct_class });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return customer.FirstOrDefault();
        }

        public async Task<AccountOpeningRequest> GetCustomerByNumber(string cust_num, string acct_class)
        {
            IEnumerable<AccountOpeningRequest> customer = new List<AccountOpeningRequest>();
            var oralConnect = new OracleConnection(_protector.Unprotect(_appSettings.FlexConnection));
            try
            {
                string query = $@"select a.*, c.account_class,a.customer_name1 customer_name,
                                 a.address_line1 D_address1,a.address_line2 D_address2,a.address_line3 D_address3,a.address_line4 D_address4,
                                 c.dr_gl,c.cr_gl,c.dr_cb_line,c.cr_cb_line,c.dr_ho_line,c.cr_ho_line,A.LOCAL_BRANCH
                                 from {_appSettings.FlexSchema}.STTM_CUSTOMER a
                                 left join {_appSettings.FlexSchema}.sttm_account_class_status c on  c.account_class = :acct_class
                                 where a.CUSTOMER_NO = :cust_num AND c.status = 'NORM'";

                using (oralConnect)
                {
                    customer = await oralConnect.QueryAsync<AccountOpeningRequest>(query, new { cust_num, acct_class });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return customer.FirstOrDefault();
        }

        public async Task<AccountOpeningResponse> GetAccountOpeningResponse(string seq_num)
        {
            IEnumerable<AccountOpeningResponse> account = new List<AccountOpeningResponse>();
            var oralConnect = new OracleConnection(_protector.Unprotect(_appSettings.FlexConnection));
            try
            {
                string query = $@"SELECT * FROM (select a.customer_no CUSTOMER_NO, b.cust_ac_no ACCOUNT_NO, B.AC_DESC CUSTOMER_NAME,
                                  B.BRANCH_CODE from {_appSettings.FlexSchema}.sttm_customer A inner join {_appSettings.FlexSchema}.sttb_upload_cust_account c 
                                  on a.customer_no=c.cust_no inner join fccuat.sttm_cust_account b on a.customer_no=b.cust_no 
                                  where maintenance_seq_no = :seq_num ORDER BY B.AC_OPEN_DATE DESC) WHERE ROWNUM < 2";

                using (oralConnect)
                {
                    account = await oralConnect.QueryAsync<AccountOpeningResponse>(query, new { seq_num });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return account.FirstOrDefault();
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

