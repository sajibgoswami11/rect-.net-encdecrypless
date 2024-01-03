using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.Dtos.TaskManagement
{
    public class TaskUsersDto
    {
		public string GroupId { get; set; }
		public string GroupParent { get; set; }
		public string GroupTitle { get; set; }
		public string GroupType { get; set; }
		public string BranchId { get; set; }
		public string BranchName { get; set; }
		public string UserId { get; set; }
		public string UserName { get; set; }
		public string UserLoginName { get; set; }
		public string UserPassword { get; set; }
		public string UserEmail { get; set; }
		public string CompanyId { get; set; }
		public string CompanyName { get; set; }
		public string PasswordExpiredate { get; set; }
		public string EmpId { get; set; }

		public string UserType { get; set; }
	}
}
