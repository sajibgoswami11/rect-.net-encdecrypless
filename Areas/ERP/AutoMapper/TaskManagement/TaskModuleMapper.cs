using AutoMapper;
using BizWebAPI.Areas.ERP.Dtos.TaskManagement;
using BizWebAPI.Areas.ERP.Models.TaskManagement;

namespace BizWebAPI.Areas.ERP.AutoMapper.TaskManagement
{
    public class TaskModuleMapper : Profile
    {
        public TaskModuleMapper()
        {
            CreateMap<TaskModuleDto, TaskModule>()
                .ForMember(dest => dest.MODULE_ID, act => act.MapFrom(src => src.ModuleId))
                .ForMember(dest => dest.MODULE_NAME, act => act.MapFrom(src => src.ModuleName))
                .ForMember(dest => dest.MILESTONES, act => act.MapFrom(src => src.Milestones))
                .ForMember(dest => dest.PROGRESS_STATUS, act => act.MapFrom(src => src.ProgressStatus))
                .ForMember(dest => dest.PROJECT_ID, act => act.MapFrom(src => src.ProjectId))
                .ForMember(dest => dest.TEAM_ID, act => act.MapFrom(src => src.TeamId));

            CreateMap<TaskModule, TaskModuleDto>()
                .ForMember(dest => dest.ModuleId, act => act.MapFrom(src => src.MODULE_ID))
                .ForMember(dest => dest.ModuleName, act => act.MapFrom(src => src.MODULE_NAME))
                .ForMember(dest => dest.Milestones, act => act.MapFrom(src => src.MILESTONES))
                .ForMember(dest => dest.ProgressStatus, act => act.MapFrom(src => src.PROGRESS_STATUS))
                .ForMember(dest => dest.ProjectId, act => act.MapFrom(src => src.PROJECT_ID))
                .ForMember(dest => dest.TeamId, act => act.MapFrom(src => src.TEAM_ID));
        }
    }
}
