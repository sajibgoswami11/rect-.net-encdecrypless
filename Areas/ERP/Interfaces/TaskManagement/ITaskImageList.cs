using BizWebAPI.Areas.ERP.Dtos;
using BizWebAPI.Areas.ERP.Dtos.TaskManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.Interfaces.TaskManagement
{
    public interface ITaskImageList
    {
        Task<string> CommonMessage();
        Task<IEnumerable<TaskImageDto>> GetTaskImageById(string taskListId, RequestDetailDto userDetails);
        Task<string> DeleteTaskImageById(string taskId, RequestDetailDto requestDetaildto);
    }
}
