using BizWebAPI.Areas.ERP.Dtos;
using BizWebAPI.Areas.ERP.Dtos.TaskManagement;
using BizWebAPI.Areas.ERP.Models.TaskManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.Interfaces.TaskManagement
{

    public interface ITaskProject
    {
        Task<string> CreateProject(TaskProjectDto projectDto, RequestDetailDto userDetails);
        Task<IEnumerable<TaskProjectDto>> GetTaskProject(RequestDetailDto userDetails);
        Task<TaskProjectDto> GetProjectById(string projectId, RequestDetailDto userDetails);
        Task<IEnumerable<TaskTeamMemberListDto>> GetProjectWiseTeamMember(string projectId, RequestDetailDto userDetails);
        Task<string> UpdateProjectByID(TaskProjectDto taskProject, RequestDetailDto userDetails);
        Task<string> CommonMessage();
        Task<string> DeleteProjectById(string statusId, RequestDetailDto userDetails);
    }
}
