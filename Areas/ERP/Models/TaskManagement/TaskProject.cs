using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.Models.TaskManagement
{
    public class TaskProject
    {
        public string PROJECT_ID { get; set; }
        public string PROJECT_NAME { get; set; }
        public string MILESTONES { get; set; }
        public string PROGRESS_STATUS { get; set; }
        public string TEAM_ID { get; set; }
        public string[] EMP_ID { get; set; }
    }
}
