using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.Models.TaskManagement
{
	public class TaskLog
	{
		public string TASK_LOG_ID { get; set; }
		public string EMP_ID { get; set; }
		public string TASK_FORWADR_ID { get; set; }
		public string TASK_ASSIGNEE_ID { get; set; }
	}
}
