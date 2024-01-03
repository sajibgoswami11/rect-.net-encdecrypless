using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.Models.TaskManagement
{
	public class TaskActivity
	{
		public string ACTIVITY_ID { get; set; }
		public DateTime START_TIME { get; set; }
		public DateTime END_TIME { get; set; }
		public DateTime TASK_ACTIVITY_DATE { get; set; }
		public string REMARKS { get; set; }
		public string ACTIVITY_IMAGE { get; set; }
		public byte[] ACTIVITY_IMAGE_SOURCE { get; set; }
		public string TASK_STATUS_LIST_ID { get; set; }
		public string EMP_ID { get; set; }
		public string TASK_ASSIGNEE_ID { get; set; }
		public string ISDELETE { get; set; }
		public string CREATE_BY { get; set; }
		public DateTime CREATE_DATE { get; set; }
		public string UPDATE_BY { get; set; }
		public DateTime UPDATE_DATE { get; set; }
		public string PROJECT_NAME { get; set; }
		public string MODULE_NAME { get; set; }
		public string STATUS_NAME { get; set; }
		public string TASK_DETAILS { get; set; }
	}
}
