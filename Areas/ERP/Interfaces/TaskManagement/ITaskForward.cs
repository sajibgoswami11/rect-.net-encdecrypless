using BizWebAPI.Areas.ERP.Dtos;
using BizWebAPI.Areas.ERP.Dtos.TaskManagement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.Interfaces.TaskManagement
{
    public interface ITaskForward
    {
        Task<string> CommonMessage();
        Task<IEnumerable<TaskForwardDto>> GetTaskForwardList(RequestDetailDto userDetails);

        Task<string> CreateTaskForward(TaskForwardDto taskForward, RequestDetailDto userDetails);

        Task<IEnumerable<TaskForwardDto>> GetTaskForwardById(string taskForwardId, RequestDetailDto userDetails);

        Task<string> UpdateTaskForwardByID(TaskForwardDto taskForward, RequestDetailDto userDetails);

        Task<string> DeleteTaskForwardById(string taskForwardId, RequestDetailDto userDetails);


    }
}
