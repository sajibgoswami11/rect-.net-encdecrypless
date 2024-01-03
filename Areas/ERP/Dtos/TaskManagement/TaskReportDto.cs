using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.Dtos.TaskManagement
{
    public class TaskReportDto
    {
        public string TaskListId { get; set; }
        public string TaskDetails { get; set; }
        public string ProjectName { get; set; }
        public string ModuleName { get; set; }
        public string ActivityStatus { get; set; }
        public string Image { get; set; }
        public string Link { get; set; }
        public string OnWork { get; set; }
        public string TimeExtendNote { get; set; }
        public string TaskRemarks { get; set; }
        public string Duration { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string EmpId { get; set; }
        public string EmpName { get; set; }
        public string AssignDate { get; set; }
        public string TaskCreatorName { get; set; }
        public string EstimateTime { get; set; }
        public string TaskDate { get; set; }
        public string ExtendTime { get; set; }
        public int TimeDuration { get; set; }
        public int TimeDurationInMinutes { get; set; }
    }
}
