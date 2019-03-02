using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountOpening.Entities
{
    public class Master
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
    public class Personal
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
    public class Customer
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
