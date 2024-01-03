using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.Repository.TaskManagement
{
    public class RoleWiseMenu
    {
        public string SYS_MENU_ID { get; set; }
        public string SYS_MENU_TITLE { get; set; }
        public string SYS_MENU_PARENT { get; set; }
        public string SYS_MENU_TYPE { get; set; }
        public string IS_ACTIVE { get; set; }
    }
}
