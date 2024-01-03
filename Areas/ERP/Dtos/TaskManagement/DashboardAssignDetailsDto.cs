using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.Dtos.TaskManagement
{
    public class DashboardAssignDetailsDto
    {
        public string ProjectName { get; set; }
        public string ModuleName { get; set; }
        public string TaskDetails { get; set; }
        public string Tasklink { get; set; }
        public string Taskdate { get; set; }
        public string AssignTo { get; set; }
        public string AssignToName { get; set; }
        public string EstimatedTime { get; set; }
        public string WorkingStatus { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int TimeDuration { get; set; }
        public int TimeDurationInMinutes { get; set; }
        public string ActivityType { get; set; }
        public string TaskAssignId { get; set; }
        public bool TimeExceed { get; set; }
    }
}
