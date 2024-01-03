using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.Dtos.TaskManagement
{
    public class TaskActivityRequestDto
    {
		public string ActivityId { get; set; }
		public string StartTime { get; set; }
		public string EndTime { get; set; }
		public string Remarks { get; set; }
		public string TaskStatusListId { get; set; }
		public string StampAdmin { get; set; }
		public string ActivityImage { get; set; }
		public string[] ActivityImageSource { get; set; }
		[Required]
		public string EmpId { get; set; }
		[Required]
		public string TaskAssigneeId { get; set; }
		public string TaskactivityDate { get; set; }
	}
}
