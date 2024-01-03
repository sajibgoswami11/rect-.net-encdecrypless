using BizWebAPI.Areas.ERP.AutoMapper;
using BizWebAPI.Areas.ERP.AutoMapper.TaskManagement;

namespace BizWebAPI.Areas.ERP.Common
{
    public class AutoMapperProfiles
    {
        public AutoMapperProfiles()
        {
            SystemUserMapper systemUserMapper = new SystemUserMapper();
            TaskActivityMapper taskActivityMapper = new TaskActivityMapper();
            SystemUserEmployeeMapper systemUserEmployeeMapper = new SystemUserEmployeeMapper();            
            TaskListMapper taskListMapper = new TaskListMapper();
            TaskProjectMapper taskProjectMapper = new TaskProjectMapper();
            TaskForwardMapper taskForwardMapper = new TaskForwardMapper();
            TaskStatusCategoryMapper taskStatusCategoryMapper = new TaskStatusCategoryMapper();
            TaskStatusListMapper taskStatusListMapper = new TaskStatusListMapper();
            TaskCreateAssignMapper taskCreateAssignMapper = new TaskCreateAssignMapper();
            TaskEmpTeamMappingMapper taskEmpTeamMappingMapper = new TaskEmpTeamMappingMapper();
            TaskTeamMemberMapper taskTeamMemberListDto = new TaskTeamMemberMapper();
            DashBoardPercentageMapper dashBoardPercentageMapper = new DashBoardPercentageMapper();
            TaskActivityRequestMapper taskActivityRequestMapper = new TaskActivityRequestMapper();
            TaskProjectProgressSummaryMapper taskProjectProgressSummaryMapper = new TaskProjectProgressSummaryMapper();
            TaskUsersMapper taskUsersMapper = new TaskUsersMapper();
            TaskTempActivitryMapper taskTempActivitryMapper = new TaskTempActivitryMapper();
            AssignTimeExtendMapper assignTimeExtendMapper = new AssignTimeExtendMapper();
            ReportsMapper reportsMapper = new ReportsMapper();
            TaskNotificationMapper taskNotificationMapper = new TaskNotificationMapper();
        }
    }
}
