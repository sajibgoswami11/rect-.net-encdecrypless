using BizWebAPI.Areas.ERP.Dtos;
using BizWebAPI.Areas.ERP.Dtos.TaskManagement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.Interfaces.TaskManagement
{
    public interface IStatusList
    {   //status list
        Task<string> CommonMessage();
        Task<IEnumerable<TaskStatusListDto>> GetStatusList(RequestDetailDto userDetails);
        Task<IEnumerable<TaskStatusListDto>> GetStatusByID(string statusId, RequestDetailDto userDetails);
        Task<IEnumerable<TaskStatusListDto>> GetCategoryWiseStatusList(string statusCategoryId, RequestDetailDto requestDetaildto);
        Task<string> CreateStatus(TaskStatusListDto taskStatusList, RequestDetailDto userDetails);
        Task<TaskStatusListDto> UpdteStatusById(TaskStatusListDto taskStatusList, RequestDetailDto userDetails);
        Task<string> DeleteStatusById(string statusId, RequestDetailDto userDetails);        
    }
}