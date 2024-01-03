using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.Models
{
    public class SystemUser
    {
        public string SYS_USR_GRP_ID { get; set; }
        public string SYS_USR_ID { get; set; }
        public string SYS_USR_DNAME { get; set; }
        public string SYS_USR_LOGIN_NAME { get; set; }
        public string SYS_USR_PASS { get; set; }
        public string SYS_USR_EMAIL { get; set; }
        public string CMP_BRANCH_ID { get; set; }
        public string ACCNT_ID { get; set; }
        public string STATUS { get; set; }
        public string OLD_DATA_ID { get; set; }
        public string USER_ACTIVE { get; set; }
        public string LOCKED_STATUS { get; set; }
        public DateTime PASSWORD_EXPIRED_DATE { get; set; }
        public string SYS_USR_PASS_PREVIOUS { get; set; }
        public string CLICK_FAILURE { get; set; }

        // System UserGroup
        public string SYS_USR_GRP_TITLE { get; set; }

        // Employee Details
        public string EMP_ID { get; set; }
        public string EMP_TITLE { get; set; }
        public string EMP_NAME { get; set; }
        public string EMP_PIC { get; set; }
        public string EMP_EMAIL { get; set; }
    }
}
