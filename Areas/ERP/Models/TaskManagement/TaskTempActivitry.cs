using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.Models.TaskManagement
{
    public class TaskTempActivitry
    {
		public string TEMPACTIVITY_ID { get; set; }
		public string EMP_ID { get; set; }
		public string TASK_ASSIGNEE_ID { get; set; }
		public string TEMPACTIVITY_TYPE { get; set; }
		public string START_TIME { get; set; }
		public string PAUSE_TIME { get; set; }
		public string TIME_EXTEND_NOTE { get; set; }
		public string ISDELETE { get; set; }
		public string CREATE_BY { get; set; }
		public DateTime CREATE_DATE { get; set; }
		public string UPDATE_BY { get; set; }
		public DateTime UPDATE_DATE { get; set; }
	}
}
