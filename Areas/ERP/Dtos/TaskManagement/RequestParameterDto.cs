using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.Dtos.TaskManagement
{
    public class TaskEmployeeId
    {
        public string EmployeeId { get; set; }
    }
    public class TaskProjectId
    {
        public string ProjectId { get; set; }
    }
    public class TaskModuleId
    {
        public string ModuleId { get; set; }
    }
    public class TaskListId
    {
        public string TaskId { get; set; }
        public string ImageId { get; set; }
    }
    public class TaskTeamId
    {
        public string TeamId { get; set; }
    }
    public class TaskActivityId
    {
        public string ActivityId { get; set; }
    }
    public class TaskActivityProjectModule
    {
        public string ProjectId { get; set; }
        public string ModuleId { get; set; }
    }
    public class TaskAssignId
    {
        public string AssignId { get; set; }
    }
    public class TaskForwardId
    {
        public string ForwardId { get; set; }
    }
    public class TaskStatusListId
    {
        public string StatusId { get; set; }
    }
    public class TaskStatusCategoryId
    {
        public string CategoryId { get; set; }
    }
    public class DashBoardDto
    {
        public string EmpId { get; set; }
        public string ProjectId { get; set; }
    }

    public class DateRangeDto
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }

    public class UserRoleDto
    {
        public string UserRoleId { get; set; }
    }
}
