using System;

namespace BizWebAPI.Areas.ERP.Models.TaskManagement
{
    public class TaskCreateAssign
    {
        public string TASKLIST_ID { get; set; }
        public DateTime TASK_DATE { get; set; }
        public string TASK_DETAILS { get; set; }
        public string IMAGE { get; set; }
        public string PROJECT_ID { get; set; }
        public string MODULE_ID { get; set; }
        public string ASSIGNEE_DATE { get; set; }
        public string PRIORITY { get; set; }
        public string EMP_ID { get; set; }
    }
}
