using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.Dtos.TaskManagement
{
    public class TaskActivityDto
	{
		public string ActivityId { get; set; }

		[Required]
		public string StartTime { get; set; }

		[Required]
		public string EndTime { get; set; }

		[Required]
		public string Remarks { get; set; }

		[Required]
		public string TaskStatusListId { get; set; }

		public string ActivityImage { get; set; }

		public string[] ActivityImageSource { get; set; }

		[Required]
		public string EmpId { get; set; }

		[Required]
		public string TaskAssigneeId { get; set; }

		[Required]
		public string TaskactivityDate { get; set; }

		public string ProjectName { get; set; }

		public string ModuleName { get; set; }

		public string Status { get; set; }

		public string TaskDetails { get; set; }
	}
}
