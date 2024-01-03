using BizWebAPI.Areas.ERP.Dtos;
using BizWebAPI.Areas.ERP.Dtos.TaskManagement;
using BizWebAPI.Areas.ERP.Models.TaskManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.Interfaces.TaskManagement
{
    public interface ITaskList
    {
        Task<string> CommonMessage();
        Task<IEnumerable<TaskListDto>> GetTaskList(RequestDetailDto userDetails);
        Task<string> CreateTask(IEnumerable<TaskCreateAssignDto> taskListDto, RequestDetailDto userDetails);

        Task<TaskListDto> GetTaskById(string taskListId, RequestDetailDto userDetails);
        Task<string> UpdateTaskByID(TaskCreateAssignDto taskListDto, RequestDetailDto userDetails);
        
        Task<string> DeleteTaskById(string taskListId, RequestDetailDto userDetails);
    }
}
