using System.ComponentModel.DataAnnotations;

namespace BizWebAPI.Areas.ERP.Dtos.TaskManagement
{
    public class TaskCreateAssignDto
    {
        public string TaskAssignId { get; set; }

        [Required]
        public string ProjectId { get; set; }

        public string ModuleId { get; set; }

        [Required]
        public string TaskDate { get; set; }

        [Required]
        public string TaskDetails { get; set; }

        public string[] Image { get; set; }

        public string ImageName { get; set; }

        [Required]
        public string Priority { get; set; }

        [Required]
        public string EstimatedTime { get; set; }

        public string TaskAssignTo { get; set; }
        public string Link { get; set; }
    }
}
