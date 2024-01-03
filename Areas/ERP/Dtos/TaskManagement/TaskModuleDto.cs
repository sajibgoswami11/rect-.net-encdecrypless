using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BizWebAPI.Areas.ERP.Dtos.TaskManagement
{
    public class TaskModuleDto
    {
        public string ModuleId { get; set; }
        [Required]
        public string ModuleName { get; set; }
        [Required]
        public string Milestones { get; set; }
        [Required]
        public string ProgressStatus { get; set; }
        [Required]
        public string ProjectId { get; set; }
        public string TeamId { get; set; }
        [Required]
        public IEnumerable<Emp> EmpList { get; set; }

    }
}
