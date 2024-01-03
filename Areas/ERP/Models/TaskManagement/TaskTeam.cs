using System;

namespace BizWebAPI.Areas.ERP.Models.TaskManagement
{
    public class TaskTeam
	{
		public string TEAM_ID { get; set; }
		public string PROJECT_ID { get; set; }
		public string MODULE_ID { get; set; }
		public string TEAM_NAME { get; set; }
		public string ISDELETE { get; set; }
		public string CREATE_BY { get; set; }
		public DateTime CREATE_DATE { get; set; }
		public string UPDATE_BY { get; set; }
		public DateTime UPDATE_DATE { get; set; }
	}
}
