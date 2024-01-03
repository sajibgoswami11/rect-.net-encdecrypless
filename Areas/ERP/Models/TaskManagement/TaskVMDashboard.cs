using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.Models.TaskManagement
{
    public class TaskVMDashboard
    {
        public string Project_Name { get; set; }
        public string Project_Id { get; set; }
        public string Total_Task { get; set; }
        public string Complete_Task { get; set; }
        public string Remaining_Task { get; set; }
        public string Closing_Ratio { get; set; }
        public string Active { get; set; }
        public string In_Progress { get; set; }
        public string Hold { get; set; }

        #region taskdetails by project
        public string TASK_DETAILS { get; set; }
        public string TEMPACTIVITY_TYPE { get; set; }
        public string ESTIMATED_TIME { get; set; }
        public string IMAGE { get; set; }
        public string ASSIGNEE_DATE { get; set; }
        public string TASK_STATUS_LIST_ID { get; set; }
        public string TASK_ASSIGNEE_ID { get; set; }
        public string EMP_NAME { get; set; }
        public string ACTIVITY_ID { get; set; }
        public string CREATE_DATE { get; set; }
        public string UPDATE_DATE { get; set; }
        public int Duration { get; set; }
        public int TimeDurationInMinutes { get; set; }
        [DataType(DataType.Date)]
        public DateTime END_TIME { get; set; }
        [DataType(DataType.Date)]
        public DateTime START_TIME { get; set; }
        #endregion
    }
}
