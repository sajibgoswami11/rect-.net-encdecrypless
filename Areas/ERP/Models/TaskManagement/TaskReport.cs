using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.Models.TaskManagement
{
    public class TaskReport
    {
        public string TASKLIST_ID { get; set; }
        public string TASK_DATE { get; set; }
        public string TASK_ASSIGNEE_ID { get; set; }
        public string STATUS_LIST_ID { get; set; }
        public string TASK_DETAILS { get; set; }
        public string PROJECT_NAME { get; set; }
        public string MODULE_NAME { get; set; }
        public string ACTIVITY_STATUS { get; set; }
        public string IMAGE { get; set; }
        public string LINK { get; set; }
        public string TEMPACTIVITY_TYPE { get; set; }
        public string TIME_EXTEND_NOTE { get; set; }
        public string REMARKS { get; set; }
        public string DURATION_OF { get; set; }
        public string START_TIME { get; set; }
        public string END_TIME { get; set; }
        public string EMP_ID { get; set; }
        public string EMP_NAME { get; set; }
        public string ASSIGNEE_DATE { get; set; }
        public string CREATE_BY { get; set; }
        public string TASKCREATOR_NAME { get; set; }
        public string ESTIMATED_TIME { get; set; }
        public string EXTEND_TIME { get; set; }
        public string ACTIVITY_ID { get; set; }
        public int DURATION_HOUR { get; set; }
        public int DURATIONINMINUTES { get; set; }
        public string COMPLETE_BY_ADMIN { get; set; }
    }
}
