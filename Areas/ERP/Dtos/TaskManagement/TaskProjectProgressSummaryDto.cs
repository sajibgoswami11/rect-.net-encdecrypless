using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.Dtos.TaskManagement
{
    public class TaskProjectProgressSummaryDto
    {

        public string Complete { get; set; }
        public string Total { get; set; }
        public string OpenTask { get; set; }
        public string Inprogress { get; set; }
        public string Remaining { get; set; }
    }
}
