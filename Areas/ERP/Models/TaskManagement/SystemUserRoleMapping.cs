using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.Models.TaskManagement
{
	public class SystemUserRoleMapping
	{
		public string SYS_USER_ROLE_MAP_ID { get; set; }
		public string SYS_USER_ROLE_ID { get; set; }
		public string SYS_USER_ID { get; set; }
		public string IS_ACTIVE { get; set; }
	}
}
