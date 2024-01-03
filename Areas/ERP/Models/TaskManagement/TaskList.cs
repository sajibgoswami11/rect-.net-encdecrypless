using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.Models.TaskManagement
{
	public class TaskList
	{
		public string EMP_ID { get; set; }
		public string TASKLIST_ID { get; set; }
		public string PROJECT_ID { get; set; }
		public string MODULE_ID { get; set; }
		public string ESTIMATED_TIME { get; set; }
		public DateTime TASK_DATE { get; set; }
		public string TASK_DETAILS { get; set; }
		public string LINK { get; set; }
		public string EXTEND_TIME { get; set; }
		public string STATUS_LIST_ID { get; set; }		
		public string IMAGE { get; set; }		
		public byte[] IMAGE_SOURCE { get; set; }		
		public string ISDELETE { get; set; }
		public string CREATE_BY { get; set; }
		public DateTime CREATE_DATE { get; set; }
		public string UPDATE_BY { get; set; }
		public DateTime UPDATE_DATE { get; set; }
		public string REFERENCE_ID { get; set; }
		public string IMAGE_PATH { get; set; }
		public string IMAGE_ID { get; set; }
	}
}
