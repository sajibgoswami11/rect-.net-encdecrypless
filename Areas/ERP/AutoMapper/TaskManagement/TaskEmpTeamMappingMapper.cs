using AutoMapper;
using BizWebAPI.Areas.ERP.Dtos.TaskManagement;
using BizWebAPI.Areas.ERP.Models.TaskManagement;

namespace BizWebAPI.Areas.ERP.AutoMapper.TaskManagement
{
    public class TaskEmpTeamMappingMapper : Profile
    {
        public TaskEmpTeamMappingMapper()
        {
            CreateMap<TaskTeamDetailsDto, TasVMkEmpTeamMapping>()
                .ForMember(dest => dest.TEAM_ID, act => act.MapFrom(src => src.TeamId))
                .ForMember(dest => dest.TEAM_NAME, act => act.MapFrom(src => src.TeamName))
                .ForMember(dest => dest.PROJECT_ID, act => act.MapFrom(src => src.ProjectId))
                .ForMember(dest => dest.MODULE_ID, act => act.MapFrom(src => src.ModuleId));

            CreateMap<TasVMkEmpTeamMapping, TaskTeamDetailsDto>()
                .ForMember(dest => dest.TeamId, act => act.MapFrom(src => src.TEAM_ID))
                .ForMember(dest => dest.TeamName, act => act.MapFrom(src => src.TEAM_NAME))
                .ForMember(dest => dest.ProjectId, act => act.MapFrom(src => src.PROJECT_ID))
                .ForMember(dest => dest.ModuleId, act => act.MapFrom(src => src.MODULE_ID));
        }
    }
}
