using BizWebAPI.Areas.ERP.Interfaces;
using BizWebAPI.Areas.ERP.Interfaces.TaskManagement;
using BizWebAPI.Areas.ERP.Manager;
using BizWebAPI.Areas.ERP.Manager.TaskManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace BizWebAPI.ConfigureServices
{
    public static class ExtractDIServices
    {
        public static void ExtractIBServices(IServiceCollection services)
        {
            services.AddScoped<IAuth, AuthManager>();
            //TASK Management
            services.AddScoped<ITaskProject, TaskProjectManager>();
            services.AddScoped<ITaskModule, TaskModuleManager>();
            services.AddScoped<IStatusList, StatusListManager>();
            services.AddScoped<ITeamEmpMapping, TeamManager>();
            services.AddScoped<ITaskList, TaskListManager>();
            services.AddScoped<ITaskAssign, TaskAssignManager>();
            services.AddScoped<ITaskActivity, TaskActivityManager>();
            services.AddScoped<ITaskForward, TaskForwardManager>();
            services.AddScoped<IDashBoard, DashBoardManager>();
            services.AddScoped<IEmployeeList, EmployeeListManager>();
            services.AddScoped<ITaskUsers, TaskUsersManager>();
            services.AddScoped<IReports, ReportsManager>();
            services.AddScoped<ITaskImageList, TaskImageListManager>();
            services.AddScoped<INotification, TaskNotificationManager>();
        }
    }
}
