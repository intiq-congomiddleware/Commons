using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountOpening.Entities
{
    public class Account
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
}
