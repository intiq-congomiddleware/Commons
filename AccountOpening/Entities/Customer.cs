using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountOpening.Entities
{
    public class Customer
    {
        public string MAINTENANCE_SEQ_NO { get; set; }
        public string CUSTOMER_NO { get; set; }
        public string CUSTOMER_TYPE { get; set; }
        public string CUSTOMER_NAME { get; set; }
        public string SHORT_NAME { get; set; }
        public string CUSTOMER_CATEGORY { get; set; }
        public string EXPOSURE_CATEGORY { get; set; }
        public string CUSTOMER_PREFIX { get; set; }
        public string FIRST_NAME { get; set; }
        public string MIDDLE_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public string DATE_OF_BIRTH { get; set; }
        public string LEGAL_GUARDIAN { get; set; }
        public string MINOR { get; set; }
        public string SEX { get; set; }
        public string NATIONAL_ID { get; set; }
        public string D_ADDRESS1 { get; set; }
        public string D_ADDRESS2 { get; set; }
        public string D_ADDRESS3 { get; set; }
        public string D_ADDRESS4 { get; set; }
        public string TELEPHONE { get; set; }
        public string FAX { get; set; }
        public string E_MAIL { get; set; }
        public string P_ADDRESS1 { get; set; }
        public string P_ADDRESS3 { get; set; }
        public string P_ADDRESS2 { get; set; }
        public string INCORP_DATE { get; set; }
        public string CAPITAL { get; set; }
        public string NETWORTH { get; set; }
        public string BUSINESS_DESCRIPTION { get; set; }
        public string AMOUNTS_CCY { get; set; }
        public string PASSPORT_NO { get; set; }
        public string PPT_ISS_DATE { get; set; }
        public string PPT_EXP_DATE { get; set; }
        public string BRANCH_CODE { get; set; }
        public string COUNTRY { get; set; }
        public string USER_ID { get; set; }
        public string NATIONALITY { get; set; }
        public string LANGUAGE { get; set; }
        public string UNIQUE_ID_NAME { get; set; }
        public string UNIQUE_ID_VALUE { get; set; }
        public string ACCOUNT_CLASS { get; set; }

        public string DR_HO_LINE { get; set; }
        public string CR_HO_LINE { get; set; }
        public string CR_CB_LINE { get; set; }
        public string DR_CB_LINE { get; set; }
        public string DR_GL { get; set; }
        public string CR_GL { get; set; }

        public bool STATUS { get; set; }
        public string ERROR_MESSAGE { get; set; }
        public string BATCH_ID { get; set; }
    }

    public class UploadMaster
    {
        public string MAINTENANCE_SEQ_NO { get; set; }
        public string BRANCH_CODE { get; set; }
        public string SOURCE_CODE { get; set; }
        public string MAINTENANCE_TYPE { get; set; }
        public string UPLOAD_STATUS { get; set; }
        public string UPLOAD_INITIATION_DATE { get; set; }
        public string USER_ID { get; set; }
        public string ACTION_CODE { get; set; }
    }
    public class UploadPersonal
    {
        public string CUSTOMER_PREFIX { get; set; }
        public string FIRST_NAME { get; set; }
        public string MIDDLE_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public string DATE_OF_BIRTH { get; set; }
        public string LEGAL_GUARDIAN { get; set; }
        public string MINOR { get; set; }
        public string SEX { get; set; }      
        public string NATIONAL_ID { get; set; }
        public string D_ADDRESS1 { get; set; }
        public string D_ADDRESS2 { get; set; }
        public string D_ADDRESS3 { get; set; }
        public string CONVERSION_STATUS_FLAG { get; set; }
        public string MAINTENANCE_SEQ_NO { get; set; }
        public string PASSPORT_NO { get; set; }
        public string PPT_ISS_DATE { get; set; }
        public string PPT_EXP_DATE { get; set; }
        public string TELEPHONE { get; set; }
        public string FAX { get; set; }
        public string E_MAIL { get; set; }
        public string P_ADDRESS1 { get; set; }
        public string P_ADDRESS3 { get; set; }
        public string P_ADDRESS2 { get; set; }
        public string CUSTOMER_NO { get; set; }
        public string D_COUNTRY { get; set; }
        public string P_COUNTRY { get; set; }
        public string RESIDENT_STATUS { get; set; }
    }
    public class UploadCustomer
    {
        public string SOURCE_CODE { get; set; }
        public string MAINTENANCE_SEQ_NO { get; set; }
        public string CUSTOMER_NO { get; set; }
        public string CUSTOMER_TYPE { get; set; }
        public string CUSTOMER_NAME1 { get; set; }
        public string ADDRESS_LINE1 { get; set; }
        public string ADDRESS_LINE3 { get; set; }
        public string ADDRESS_LINE2 { get; set; }
        public string ADDRESS_LINE4 { get; set; }
        public string COUNTRY { get; set; }
        public string SHORT_NAME { get; set; }
        public string NATIONALITY { get; set; }
        public string LANGUAGE { get; set; }
        public string EXPOSURE_COUNTRY { get; set; }
        public string LOCAL_BRANCH { get; set; }
        public string LIABILITY_NO { get; set; }
        public string FROZEN { get; set; }
        public string DECEASED { get; set; }
        public string WHEREABOUTS_UNKNOWN { get; set; }
        public string CUSTOMER_CATEGORY { get; set; }
        public string FX_CUST_CLEAN_RISK_LIMIT { get; set; }
        public string OVERALL_LIMIT { get; set; }
        public string FX_CLEAN_RISK_LIMIT { get; set; }
        public string CREDIT_RATING { get; set; }
        public string REVISION_DATE { get; set; }
        public string LIMIT_CCY { get; set; }
        public string CAS_CUST { get; set; }
        public string CONVERSION_STATUS_FLAG { get; set; }
        public string SEC_CLEAN_RISK_LIMIT { get; set; }
        public string SEC_CUST_PSTL_RISK_LIMIT { get; set; }
        public string SEC_PSTL_RISK_LIMIT { get; set; }
        public string SEC_CUST_CLEAN_RISK_LIMIT { get; set; }
        public string SWIFT_CODE { get; set; }
        public string LIAB_BR { get; set; }
        public string LIAB_NODE { get; set; }
        public string DEFAULT_MEDIA { get; set; }
        public string SHORT_NAME2 { get; set; }
        public string SSN { get; set; }
        public string ACTION_CODE { get; set; }
        public string UTILITY_PROVIDER { get; set; }
        public string MAILERS_REQUIRED { get; set; }
        public string AML_REQUIRED { get; set; }
        public string FT_ACCTING_AS_OF { get; set; }
        public string CUST_UNADVISED { get; set; }
        public string LIAB_UNADVISED { get; set; }
        public string CONSOL_TAX_CERT_REQD { get; set; }
        public string INDIVIDUAL_TAX_CERT_REQD { get; set; }
        public string UNIQUE_ID_NAME { get; set; }
        public string UNIQUE_ID_VALUE { get; set; }
        public string FAX_NUMBER { get; set; }
        public string LOC_CODE { get; set; }
    }

    public class UploadAccount
    {
        public string MAINTENANCE_SEQ_NO { get; set; }
        public string SOURCE_CODE { get; set; }
        public string BRANCH_CODE { get; set; }
        public string CUST_AC_NO { get; set; }
        public string AC_DESC { get; set; }
        public string CUST_NO { get; set; }
        public string CCY { get; set; }
        public string ACCOUNT_CLASS { get; set; }
        public string AC_STAT_NO_DR { get; set; }
        public string AC_STAT_NO_CR { get; set; }
        public string AC_STAT_BLOCK { get; set; }
        public string AC_STAT_STOP_PAY { get; set; }
        public string AC_STAT_DORMANT { get; set; }
        public DateTime AC_OPEN_DATE { get; set; }
        public int AC_STMT_DAY { get; set; }
        public string AC_STMT_CYCLE { get; set; }
        public string CHEQUE_BOOK_FACILITY { get; set; }
        public string DR_HO_LINE { get; set; }
        public string CR_HO_LINE { get; set; }
        public string CR_CB_LINE { get; set; }
        public string DR_CB_LINE { get; set; }
        public string DR_GL { get; set; }
        public string CR_GL { get; set; }
        public string ADDRESS1 { get; set; }
        public string ADDRESS2 { get; set; }
        public string ADDRESS3 { get; set; }
        public string ADDRESS4 { get; set; }
        public string ACC_STATUS { get; set; }
        public DateTime STATUS_SINCE { get; set; }
        public string INHERIT_REPORTING { get; set; }
        public string ACTION_CODE { get; set; }
        public string JOINT_AC_INDICATOR { get; set; }
        public string ATM_FACILITY { get; set; }
        public string PASSBOOK_FACILITY { get; set; }
        public string AC_STMT_TYPE { get; set; }
        public int ACC_STMT_DAY2 { get; set; }
        public string AC_STMT_CYCLE2 { get; set; }
        public string ACC_STMT_TYPE2 { get; set; }
        public string GEN_STMT_ONLY_ON_MVMT2 { get; set; }
        public string GEN_STMT_ONLY_ON_MVMT3 { get; set; }
        public int ACC_STMT_DAY3 { get; set; }
        public string AC_STMT_CYCLE3 { get; set; }
        public string ACC_STMT_TYPE3 { get; set; }
        public string TYPE_OF_CHQ { get; set; }

        public string GEN_STMT_ONLY_ON_MVMT { get; set; }
        public string AC_STAT_DE_POST { get; set; }
        public string DISPLAY_IBAN_IN_ADVICES { get; set; }
        public string REG_CC_AVAILABILITY { get; set; }
        public string MT210_REQD { get; set; }
        public string SWEEP_TYPE { get; set; }
        public string DORMANT_PARAM { get; set; }
        public string POSITIVE_PAY_AC { get; set; }
        public string TRACK_RECEIVABLE { get; set; }

        public string NETTING_REQUIRED { get; set; }
        public string LODGEMENT_BOOK_FACILITY { get; set; }
        public string REFERRAL_REQUIRED { get; set; }
        public string EXCL_SAMEDAY_RVRTRNS_FM_STMT { get; set; }
        public string ALLOW_BACK_PERIOD_ENTRY { get; set; }

        public string PROV_CCY_TYPE { get; set; }
        public string DEFER_RECON { get; set; }
        public string CONSOLIDATION_REQD { get; set; }
        public string FUNDING { get; set; }
        public string ALT_AC_NO { get; set; }
        public string AUTO_PROV_REQD { get; set; }
    }
    public class ExecuteCustomer
    {
        public string BranchCode { get; set; }
        public string SourceCode { get; set; }
        public string UserId { get; set; }
        public string BranchNode { get; set; }
    }
    public class CustomerNo
    {
        public string Customer_No { get; set; }
    }
}
