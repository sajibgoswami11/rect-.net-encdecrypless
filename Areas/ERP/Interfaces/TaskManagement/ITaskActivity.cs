using BizWebAPI.Areas.ERP.Dtos;
using BizWebAPI.Areas.ERP.Dtos.TaskManagement;
using BizWebAPI.Areas.ERP.Models.TaskManagement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.Interfaces.TaskManagement
{
    public interface ITaskActivity
    {
        Task<string> CommonMessage();
        Task<IEnumerable<TaskActivityDto>> GetTaskActivityList(RequestDetailDto requestDetaildto);
        Task<string> CreateTaskActivity(TaskActivityRequestDto taskTaskActivityDto, RequestDetailDto requestDetaildto);
        Task<IEnumerable<TaskActivityDto>> GetTaskActivityById(string taskActivityId, RequestDetailDto requestDetaildto);
        Task<IEnumerable<TaskActivityDto>> TaskActivityByProjectModule(TaskActivityProjectModule ProjectModule, RequestDetailDto requestDetaildto);
        Task<string> UpdateTaskActivityByID(TaskActivityDto taskTaskActivityDto, RequestDetailDto requestDetaildto);
        Task<string> DeleteTaskActivityById(string taskActivityId, RequestDetailDto requestDetaildto);

        Task<IEnumerable<DashboardAssignDetailsDto>> GetTaskCompletebyAdmin(RequestDetailDto requestDetaildto);
    }
}
