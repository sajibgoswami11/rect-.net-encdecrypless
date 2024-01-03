using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.Models.TaskManagement
{
	public class TaskNotification
	{
		public string NOTIFICATION_ID { get; set; }
		public string TITLE { get; set; }
		public string EXTEND_TIME { get; set; }
		public string MESSAGE { get; set; }
		public string SEEN_STATUS { get; set; }
		public string SEEN_BY { get; set; }
		public string LINK { get; set; }
		public string CREATE_BY { get; set; }
		public DateTime CREATE_DATE { get; set; }
		public string UPDATE_BY { get; set; }
		public DateTime UPDATE_DATE { get; set; }
		public string ISDELETE { get; set; }
	}
}

