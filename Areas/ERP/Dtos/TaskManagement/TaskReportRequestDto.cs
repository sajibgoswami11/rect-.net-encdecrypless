using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.Dtos.TaskManagement
{
    public class TaskReportRequestDto
    {
        public string ProjectId { get; set; }
        public string ModuleId { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string EmployeeId { get; set; }
        public string ActivityStatus { get; set; }
        public IEnumerable<Emp> EmpList { get; set; }

    }
}
