using AutoMapper;
using BizWebAPI.Areas.ERP.Dtos.TaskManagement;
using BizWebAPI.Areas.ERP.Models.TaskManagement;

namespace BizWebAPI.Areas.ERP.AutoMapper.TaskManagement
{
    public class TaskCreateAssignMapper : Profile
    {
        public TaskCreateAssignMapper()
        {
            CreateMap<TaskCreateAssignDto, TaskCreateAssign>()
                .ForMember(dest => dest.TASK_DATE, act => act.MapFrom(src => src.TaskDate))
                .ForMember(dest => dest.TASK_DETAILS, act => act.MapFrom(src => src.TaskDetails))
                .ForMember(dest => dest.IMAGE, act => act.MapFrom(src => src.Image))
                .ForMember(dest => dest.PROJECT_ID, act => act.MapFrom(src => src.ProjectId))
                .ForMember(dest => dest.MODULE_ID, act => act.MapFrom(src => src.ModuleId))
                .ForMember(dest => dest.EMP_ID, act => act.MapFrom(src => src.TaskAssignTo))
                .ForMember(dest => dest.PRIORITY, act => act.MapFrom(src => src.Priority));

            CreateMap<TaskCreateAssign, TaskCreateAssignDto>()
                .ForMember(dest => dest.TaskDate, act => act.MapFrom(src => src.TASK_DATE))
                .ForMember(dest => dest.TaskDetails, act => act.MapFrom(src => src.TASK_DETAILS))
                .ForMember(dest => dest.Image, act => act.MapFrom(src => src.IMAGE))
                .ForMember(dest => dest.ProjectId, act => act.MapFrom(src => src.PROJECT_ID))
                .ForMember(dest => dest.ModuleId, act => act.MapFrom(src => src.MODULE_ID))
                .ForMember(dest => dest.TaskAssignTo, act => act.MapFrom(src => src.EMP_ID))
                .ForMember(dest => dest.Priority, act => act.MapFrom(src => src.PRIORITY));
        }
    }
}
