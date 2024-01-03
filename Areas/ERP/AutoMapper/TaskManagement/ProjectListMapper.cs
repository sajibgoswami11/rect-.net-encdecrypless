
using BizWebAPI.Areas.ERP.Dtos.TaskManagement;
using BizWebAPI.Areas.ERP.Models.TaskManagement;
using AutoMapper;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.AutoMapper.TaskManagement
{
    public class ProjectListMapper : Profile
    {
        public ProjectListMapper()
        {
            CreateMap<TaskProjectDto, TaskProject>()
                .ForMember(dest => dest.PROJECT_NAME, act => act.MapFrom(src => src.ProjectName))
                .ForMember(dest => dest.PROGRESS_STATUS, act => act.MapFrom(src => src.ProgressStatus))
                .ForMember(dest => dest.MILESTONES, act => act.MapFrom(src => src.Milestone))
                .ForMember(dest => dest.TEAM_ID, act => act.MapFrom(src => src.TeamId));

            CreateMap<TaskProject,TaskProjectDto >()
                .ForMember(dest => dest.ProjectName, act => act.MapFrom(src => src.PROJECT_NAME))
                .ForMember(dest => dest.ProgressStatus, act => act.MapFrom(src => src.PROGRESS_STATUS))
                .ForMember(dest => dest.Milestone, act => act.MapFrom(src => src.MILESTONES))
                .ForMember(dest => dest.TeamId, act => act.MapFrom(src => src.TEAM_ID));
        }
    }
} 