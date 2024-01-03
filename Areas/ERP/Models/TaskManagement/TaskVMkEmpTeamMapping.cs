using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.Models.TaskManagement
{
	public class TasVMkEmpTeamMapping
	{
		public string TEAM_ID { get; set; }
		public string TEAM_NAME { get; set; }
		public string PROJECT_ID { get; set; }
		public string MODULE_ID { get; set; }		
		public string EMP_NAME { get; set; }

		[NotMapped]
		public string EMP_ID { get; set; }

		[NotMapped]
		public string IMAGE { get; set; }

		[NotMapped]
		public string[] EmpList { get; set; }
	}
}
