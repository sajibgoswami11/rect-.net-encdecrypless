using BizWebAPI.Areas.ERP.Dtos.TaskManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BizWebAPI.Areas.ERP.Models.TaskManagement
{
    public class TaskVMHelper
    {
		public string TaskListId{ get; set; }
		public DateTime TaskDate { get; set; }
		public string TaskDetails { get; set; }
		public string Image { get; set; }
		public string ProjectId { get; set; }
		public string ModuleId { get; set; }
		public string TaskAssignId { get; set; }
		public DateTime AssignDate { get; set; }
		public string Priority { get; set; }
		public string EmpId { get; set; }
		[NotMapped]
		public string[] EmpIdList { get; set; }
		public byte[] TaskImage { get; set; }

		// public TaskCreateAssignDto TaskCreateAssignList { get; set; }
	}
}
