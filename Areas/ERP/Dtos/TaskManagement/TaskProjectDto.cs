using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BizWebAPI.Areas.ERP.Dtos.TaskManagement
{
    public class TaskProjectDto
    {
        public string ProjectId { get; set; }
        [Required]
        public string ProjectName { get; set; }
        [Required]
        public string Milestone { get; set; }
        [Required]
        public string ProgressStatus { get; set; }
        public string TeamId { get; set; }
        [Required]
        public IEnumerable<Emp> EmpList { get; set; }
    }

}
