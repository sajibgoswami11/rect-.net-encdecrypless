using System;
using System.ComponentModel.DataAnnotations;

namespace BizWebAPI.Areas.ERP.Dtos.TaskManagement
{
    public class TaskForwardDto
    {
		public string TaskForwardId { get; set; }

		[Required]
		public string TaskAssigneeId { get; set; }
		public string TaskDetails { get; set; }

		[Required]
		public string EmpId { get; set; }
		public string TaskForwardDate { get; set; }
		public string Priority { get; set; }
	}
}
