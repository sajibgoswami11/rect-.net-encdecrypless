using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.Dtos.TaskManagement
{
    public class TaskTempActivitryDto
    {
        public string EmpId { get; set; }
        public string TempActivityId { get; set; }
        public string TempActivityType { get; set; }
        public string TaskAssignId { get; set; }
        public string StartTime { get; set; }
        public string PauseTime { get; set; }
    }
}
