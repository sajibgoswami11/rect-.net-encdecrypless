using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.Models.TaskManagement
{
    public class TaskStatusList
    {
		public string TASK_STATUS_LIST_ID { get; set; }
		public string STATUS_NAME { get; set; }
		public string PROGRESS_STATUS { get; set; }
		public string STATUS_CATEGORY_ID { get; set; }
		public string ISDELETE { get; set; }
		public string CREATE_BY { get; set; }
		public DateTime CREATE_DATE { get; set; }
		public string UPDATE_BY { get; set; }
		public DateTime UPDATE_DATE { get; set; }
		[NotMapped]
		public string CATEGORY_NAME { get; set; }
	}
}
