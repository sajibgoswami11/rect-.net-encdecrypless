using System;

namespace BizWebAPI.Areas.ERP.Models.TaskManagement
{
    public class TaskModule
    {
        public string MODULE_ID {get;set;}
        public string MODULE_NAME { get;set;}
        public string MILESTONES { get;set;}
        public string PROGRESS_STATUS { get;set;}
        public string PROJECT_ID { get;set;}
		public string TEAM_ID { get; set; }
        public string[] EMP_ID { get; set; }

    }
}