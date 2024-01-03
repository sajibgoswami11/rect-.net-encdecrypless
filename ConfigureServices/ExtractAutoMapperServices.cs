using BizWebAPI.Areas.ERP.Manager;
using BizWebAPI.Areas.ERP.Manager.TaskManagement;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace BizWebAPI.ConfigureServices
{
    public static class ExtractAutoMapperServices
    {
        internal static void ExtractIBAutoMapper(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AuthManager).Assembly);
            //TASKMGT
            services.AddAutoMapper(typeof(TaskProjectManager).Assembly);
            services.AddAutoMapper(typeof(TaskListManager).Assembly);
            services.AddAutoMapper(typeof(TaskAssignManager).Assembly);
        }
    }
}
