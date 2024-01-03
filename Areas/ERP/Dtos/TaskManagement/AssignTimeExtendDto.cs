using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.Dtos.TaskManagement
{
    public class AssignTimeExtendDto
    {
        [Required]
        public string TaskAssignId { get; set; }
        [Required]
        public string TimeExtendNote { get; set; }
        public string EmpId { get; set; }
        public string ExtendTime { get; set; }
    }
}
