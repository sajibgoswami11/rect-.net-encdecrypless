using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.Dtos.TaskManagement
{
    public class RoleWiseMenuDto
    {
        public string Text { get; set; }
        public string Value { get; set; }
        public string Disabled { get; set; }
        public bool Checked { get; set; }
        public bool Collapsed { get; set; }
      
        public List<RoleWiseMenuDto> Children { get; set; }
    }
}
