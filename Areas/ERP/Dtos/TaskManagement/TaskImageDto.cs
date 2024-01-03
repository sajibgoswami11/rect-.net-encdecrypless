using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.Dtos.TaskManagement
{
    public class TaskImageDto
    {
        public string ImageId { get; set; }
        public string ImagePath { get; set; }
        public string ReferenceId { get; set; }
        public string IsDelete { get; set; }

    }
}
