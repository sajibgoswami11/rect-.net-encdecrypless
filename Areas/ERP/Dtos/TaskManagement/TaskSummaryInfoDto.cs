using AutoMapper;
using System.Collections.Generic;

namespace BizWebAPI.Areas.ERP.Dtos.TaskManagement
{
    public class TaskSummaryInfoDto
    {
        public IEnumerable<TaskProjectDto> ProjectDetails { get; set; }
        public IEnumerable<TaskModuleDto> ModuleDetails { get; set; }
        public IEnumerable<TaskAssignDto> TaskAssignDetails { get; set; }
        public IEnumerable<TaskCreateAssignDto> TaskListDetails { get; set; }
        public IEnumerable<DashBoardPercentageDto> DashboardPercentage { get; set; }
    }
}
