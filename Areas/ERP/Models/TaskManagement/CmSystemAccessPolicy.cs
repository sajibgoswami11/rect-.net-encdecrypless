using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.Models.TaskManagement
{
    public class CmSystemAccessPolicy
    {
        [Key]
        [Display(Name = "ID")]
        public string SYS_ACCP_ID { get; set; }

        [Display(Name = "User Group")]
        public string SYS_USR_GRP_ID { get; set; }
        public string SYS_USR_ID { get; set; }
        public string SYS_USR_LOGIN_NAME { get; set; }

        public string SYS_USR_GRP_TITLE { get; set; }
        public string SYS_MENU_TYPE { get; set; }

        [Display(Name = "Menu")]
        public string SYS_MENU_ID { get; set; }
        public string SYS_MENU_PARENT { get; set; }
        public string SYS_MENU_SERIAL { get; set; }

        public string SYS_MENU_FILE { get; set; }

        public string SYS_MENU_TITLE { get; set; }

        public string PATH_NAME { get; set; }
        public string MENU_ICON { get; set; }

        [Display(Name = "View")]
        public string SYS_ACCP_VIEW { get; set; }

        [Display(Name = "Add")]
        public string SYS_ACCP_ADD { get; set; }

        [Display(Name = "Edit")]
        public string SYS_ACCP_EDIT { get; set; }

        [Display(Name = "Delete")]
        public string SYS_ACCP_DELETE { get; set; }

        public string DPTDSWISE_ID { get; set; }
        public string COMPANY_ID { get; set; }
        public string SYSTEM_ROLE_MENU_MAP_ID { get; set; }

        [NotMapped]
        public string[] OtherList { get; set; }

        public string SYS_USER_ROLE_MAP_ID { get; set; }

    }
}
