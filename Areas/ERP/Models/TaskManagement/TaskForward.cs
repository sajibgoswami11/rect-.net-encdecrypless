using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.Models.TaskManagement
{
	public class TaskForward
	{
		public string TASK_FORWADR_ID { get; set; }
		public string TASK_ASSIGNEE_ID { get; set; }
		public string TASK_DETAILS { get; set; }
		public string EMP_ID { get; set; }
		public string TASK_FORWARD_DATE { get; set; }
		public string PRIORITY { get; set; }
		public string ISDELETE { get; set; }
		public string CREATE_BY { get; set; }
		public DateTime CREATE_DATE { get; set; }
		public string UPDATE_BY { get; set; }
		public DateTime UPDATE_DATE { get; set; }
	}
}
