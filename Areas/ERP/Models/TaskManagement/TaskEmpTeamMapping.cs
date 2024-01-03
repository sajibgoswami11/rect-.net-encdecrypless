using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.Models.TaskManagement
{
    public class TaskEmpTeamMapping
    {
		public string GROUP_MAPPING_ID { get; set; }
		public string TEAM_ID { get; set; }
		public string EMP_ID { get; set; }
		public string ISDELETE { get; set; }
		public string PROJECT_ID { get; set; }
		public string MODULE_ID { get; set; }
	}
}
