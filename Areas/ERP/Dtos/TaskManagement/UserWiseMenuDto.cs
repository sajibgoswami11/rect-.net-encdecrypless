using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.Dtos.TaskManagement
{
    public class UserWiseMenuDto
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public List<UserWiseMenuDto> Children { get; set; }

    }
}
