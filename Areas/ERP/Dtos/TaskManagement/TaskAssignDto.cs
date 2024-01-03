using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.Dtos.TaskManagement
{
    public class TaskAssignDto
    {
		public string TaskAssignId { get; set; }
		public string TaskListId { get; set; }
		public string TaskDetails { get; set; }
		public string AssignDate { get; set; }
		public string Priority { get; set; }
		public string EmployeeId { get; set; }
		public string AssignById { get; set; }
		public string AssignByName { get; set; }
		public string AssignByImage { get; set; }
		public string ProjectName { get; set; }
		public string ModuleName { get; set; }
		public string EstimateTime { get; set; }
		public string TaskImage { get; set; }
		public string TempActivityType { get; set; }
		public string TimeExtendNote { get; set; }
		public bool TimeExceed { get; set; }
	}
}
