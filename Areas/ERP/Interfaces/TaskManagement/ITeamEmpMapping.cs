using BizWebAPI.Areas.ERP.Dtos;
using BizWebAPI.Areas.ERP.Dtos.TaskManagement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.Interfaces.TaskManagement
{
    public interface ITeamEmpMapping
    {
        Task<string> CommonMessage();
        Task<IEnumerable<TaskTeamDetailsDto>> GetTaskTeams(RequestDetailDto requestDetaildto);
        Task<string> CreateTeam(TeamCreateDto taskTeamDetails, RequestDetailDto userDetails);
        Task<string> UpdateTeam(TeamCreateDto taskTeamDto, RequestDetailDto requestDetaildto);
        Task<string> DeleteTaskTeams(string teamId, RequestDetailDto requestDetaildto);
        Task<IEnumerable<TaskTeamDetailsDto>> GetTaskTeamById(string teamId, RequestDetailDto userDetails);
        Task<IEnumerable<TaskEmpTeamMappingDto>> GetTeamWiseEmployeeMapping(string TeamId, RequestDetailDto requestDetaildto);






        //Task<string> CreateEmpTeamMapping(TaskEmpTeamMappingDto empTeamMaping, RequestDetailDto requestDetaildto);
        //Task<string> CreateEmpTeamMapping(TaskEmpTeamMappingDto empTeamMaping, RequestDetailDto requestDetaildto);
        //Task<IEnumerable<TaskEmpTeamMappingDto>> UpdateEmpTeamMap(TaskEmpTeamMappingDto taskEmpTeamMappingDto, RequestDetailDto userDetails);
        //Task<string> DeleteEmpTeamMap(string TeamMappingId, RequestDetailDto userDetails);
        //Task<IEnumerable<TeamWiseEmployeeMappingDto>> GetTeamWiseEmployeeMappingById(string teamMappingId, RequestDetailDto requestDetaildto);
        
        
        
        
        
    }
}
