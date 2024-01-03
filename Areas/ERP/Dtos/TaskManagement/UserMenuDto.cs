using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.Dtos.TaskManagement
{
    public class UserMenuDto
    {
        public string UserId { get; set; }

        public IEnumerable<Menu> MenuId { get; set; }
    }
}
