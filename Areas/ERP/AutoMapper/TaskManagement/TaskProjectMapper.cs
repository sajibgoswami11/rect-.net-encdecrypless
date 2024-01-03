using AutoMapper;
using BizWebAPI.Areas.ERP.Dtos.TaskManagement;
using BizWebAPI.Areas.ERP.Models.TaskManagement;

namespace BizWebAPI.Areas.ERP.AutoMapper.TaskManagement
{
    public class TaskProjectMapper : Profile
    {
        public TaskProjectMapper()
        {
            CreateMap<TaskProjectDto, TaskProject>()
                .ForMember(dest => dest.PROJECT_ID, act => act.MapFrom(src => src.ProjectId))
                .ForMember(dest => dest.PROJECT_NAME, act => act.MapFrom(src => src.ProjectName))
                .ForMember(dest => dest.MILESTONES, act => act.MapFrom(src => src.Milestone))
                .ForMember(dest => dest.PROGRESS_STATUS, act => act.MapFrom(src => src.ProgressStatus))
                .ForMember(dest => dest.TEAM_ID, act => act.MapFrom(src => src.TeamId))
                .ForMember(dest => dest.EMP_ID, act => act.MapFrom(src => src.EmpList))
                ;

            CreateMap<TaskProject, TaskProjectDto>()
                .ForMember(dest => dest.ProjectId, act => act.MapFrom(src => src.PROJECT_ID))
                .ForMember(dest => dest.ProjectName, act => act.MapFrom(src => src.PROJECT_NAME))
                .ForMember(dest => dest.Milestone, act => act.MapFrom(src => src.MILESTONES))
                .ForMember(dest => dest.ProgressStatus, act => act.MapFrom(src => src.PROGRESS_STATUS))
                .ForMember(dest => dest.TeamId, act => act.MapFrom(src => src.TEAM_ID))
                .ForMember(dest => dest.EmpList, act => act.MapFrom(src => src.EMP_ID))
                ;
        }
    }
}
