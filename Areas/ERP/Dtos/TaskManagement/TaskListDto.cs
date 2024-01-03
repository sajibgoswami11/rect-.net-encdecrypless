using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.Dtos.TaskManagement
{
    public class TaskListDto
    {
		public string EmpId { get; set; }
		public string TaskListId { get; set; }
		public string ProjectId { get; set; }
		public string ModuleId { get; set; }
		public string EstimatedTime{ get; set; }
		public DateTime TaskDate { get; set; }
		public string TaskDetails { get; set; }
		public string Link { get; set; }
		public string Image { get; set; }
		public string StatusListId { get; set; }
		public byte[] ImageSource { get; set; }
		public string ImagePathTofolder { get; set; }
		public string ImageId { get; set; }
		public List<string> ImagePathUrls { get; set; }
	}
}
