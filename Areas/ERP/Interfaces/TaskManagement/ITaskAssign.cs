using BizWebAPI.Areas.ERP.Dtos;
using BizWebAPI.Areas.ERP.Dtos.TaskManagement;
using BizWebAPI.Areas.ERP.Models.TaskManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.Interfaces.TaskManagement
{
    public interface ITaskAssign
    {
        Task<string> CommonMessage();
        Task<IEnumerable<TaskAssignDto>> GetTaskAssignList(RequestDetailDto userDetails);
        Task<IEnumerable<TaskAssignDto>> GetTaskAssignWiseName(RequestDetailDto userDetails);
        Task<string> CreateTaskAssign(TaskAssignDto taskTaskAssignDto, RequestDetailDto userDetails);

        Task<IEnumerable<TaskAssignDto>> GetTaskAssignById(string taskAssignId, RequestDetailDto userDetails);

        Task<IEnumerable<TaskAssignDto>> UpdateTaskAssignByID(TaskAssignDto taskTaskAssignDto, RequestDetailDto userDetails);
        Task<string> TaskSatarPause(TaskTempActivitryDto tempActivity, RequestDetailDto userDetails);

        Task<string> DeleteTaskAssignById(string taskAssignId, RequestDetailDto userDetails);
        Task<string> AssignTimeExtend(AssignTimeExtendDto assignTimeExtend, RequestDetailDto userDetails);
    }
}
