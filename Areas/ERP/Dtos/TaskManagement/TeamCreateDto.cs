using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.Dtos.TaskManagement
{
    public class TeamCreateDto
    {
		public string TeamId { get; set; }
		public string TeamName { get; set; }
		public IEnumerable<Emp> EmpList { get; set; }
	}

	public class Emp
	{
		[Required]
		public string emp { get; set; }
	}
}
