using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.Dtos.TaskManagement
{
    public class ProjectModuleTeamDto
    {
        public string ProjectName { get; set; }
        public string ProjectId { get; set; }
        public string ProgressStat { get; set; }
        public string Milestones { get; set; }
        public string ModuleId { get; set; }
        public string ModuleName { get; set; }
        public string ModuleMilestones { get; set; }
        public string ModuleProgressStat { get; set; }
        public string ModuleProjectId { get; set; }
        public string TeamId { get; set; }
    }
}
