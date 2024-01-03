using BizWebAPI.Areas.ERP.Dtos;
using BizWebAPI.Areas.ERP.Dtos.TaskManagement;
using BizWebAPI.Areas.ERP.Models.TaskManagement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.Interfaces.TaskManagement
{
    public interface ITaskModule
    {
        Task<string> CreateModule(IEnumerable<TaskModuleDto> taskModuleDto, RequestDetailDto requestDetailDto);
        Task<IEnumerable<TaskModuleDto>> GetTaskModule(RequestDetailDto requestDetailDto);
        Task<IEnumerable<TaskModuleDto>> GetModuleById(string moduleId, RequestDetailDto requestDetailDto);
        Task<IEnumerable<ErpAuthLoginDto>> GetPersonsOfModuleByAssignId(string empId, RequestDetailDto requestDetailDto);
        Task<IEnumerable<TaskModuleDto>> GetModuleListByProjectId(string projectId, RequestDetailDto requestDetailDto);
        Task<IEnumerable<TaskEmpTeamMappingDto>> GetModuleWiseTeamMember(string moduleId, RequestDetailDto requestDetaildto);
        Task<string> UpdateModuleByID(TaskModuleDto taskModuleDto, RequestDetailDto requestDetailDto);
        Task<string> CommonMessage();
        Task<string> DeleteModuleById(string moduleId, RequestDetailDto requestDetailDto);
    }
}
