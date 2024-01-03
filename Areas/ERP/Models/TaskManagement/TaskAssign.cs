using System;

namespace BizWebAPI.Areas.ERP.Models.TaskManagement
{
    public class TaskAssign
	{
		public string TASK_ASSIGNEE_ID { get; set; }
		public string TASKLIST_ID { get; set; }
		public string TASK_DETAILS { get; set; }
		public string EMP_ID { get; set; }
		public DateTime ASSIGNEE_DATE { get; set; }
		public string PRIORITY { get; set; }
		public string ISDELETE { get; set; }
		public string CREATE_BY { get; set; }
		public DateTime CREATE_DATE { get; set; }
		public string UPDATE_BY { get; set; }
		public DateTime UPDATE_DATE { get; set; }
		public string PROJECT_ID { get; set; }
		public string PROJECT_NAME { get; set; }
		public string MODULE_NAME { get; set; }
		public string ESTIMATED_TIME { get; set; }
		public string TEMPACTIVITY_TYPE { get; set; }
		public string TIME_EXTEND_NOTE { get; set; }
		public bool TIME_EXCEED { get; set; }

		//relation data for EmployeeList
		public string EMP_NAME { get; set; }
		public string IMAGE { get; set; }
		public string TASK_IMAGE { get; set; }
		public string LINK { get; set; }

	}
}
