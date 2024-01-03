using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.Dtos.TaskManagement
{
    public class AccessPolicyDto
    {
        public string UserGroupId {get; set;}
        public string UserGroupTitle {get; set;}
        public string UserMenuId {get; set;}
        public string SysRoleMenuMapId {get; set;}
        public string MenuType { get; set;}
        public string MenuSerial { get; set;}
        public string ParentMenuId {get; set;}
        public string UserId {get; set;}
        public string UserName {get; set;}
        public string UserMenuTitle {get; set;}
        public string UserMenuFile {get; set;}
        public IEnumerable<Menu> UserMenuList { get; set; }
        public string MenuIcon {get; set;}

    }

    public class Menu
    {
        public string UserMenuId { get; set; }
    }
}
