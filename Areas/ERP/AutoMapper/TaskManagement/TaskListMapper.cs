using BizWebAPI.Areas.ERP.Dtos.TaskManagement;
using BizWebAPI.Areas.ERP.Models.TaskManagement;
using AutoMapper;

namespace BizWebAPI.Areas.ERP.AutoMapper.TaskManagement
{
    public class TaskListMapper : Profile
    {
        public TaskListMapper()
        {
            CreateMap<TaskListDto, TaskList>()
                .ForMember(dest => dest.TASKLIST_ID, act => act.MapFrom(src => src.TaskListId))
                .ForMember(dest => dest.PROJECT_ID, act => act.MapFrom(src => src.ProjectId))
                .ForMember(dest => dest.MODULE_ID, act => act.MapFrom(src => src.ModuleId))
                .ForMember(dest => dest.TASK_DATE, act => act.MapFrom(src => src.TaskDate))
                .ForMember(dest => dest.TASK_DETAILS, act => act.MapFrom(src => src.TaskDetails))
                .ForMember(dest => dest.ESTIMATED_TIME, act => act.MapFrom(src => src.EstimatedTime))
                .ForMember(dest => dest.IMAGE, act => act.MapFrom(src => src.Image))
                .ForMember(dest => dest.IMAGE_SOURCE, act => act.MapFrom(src => src.ImageSource))
                .ForMember(dest => dest.LINK, act => act.MapFrom(src => src.Link))
                .ForMember(dest => dest.EMP_ID, act => act.MapFrom(src => src.EmpId))
                .ForMember(dest => dest.STATUS_LIST_ID, act => act.MapFrom(src => src.StatusListId))
                .ForMember(dest => dest.IMAGE_PATH, act => act.MapFrom(src => src.ImagePathTofolder))
                .ForMember(dest => dest.IMAGE_ID, act => act.MapFrom(src => src.ImageId))
                ;

            CreateMap<TaskList, TaskListDto>()
                .ForMember(dest => dest.TaskListId, act => act.MapFrom(src => src.TASKLIST_ID))
                .ForMember(dest => dest.ProjectId, act => act.MapFrom(src => src.PROJECT_ID))
                .ForMember(dest => dest.ModuleId, act => act.MapFrom(src => src.MODULE_ID))
                .ForMember(dest => dest.TaskDate, act => act.MapFrom(src => src.TASK_DATE))
                .ForMember(dest => dest.TaskDetails, act => act.MapFrom(src => src.TASK_DETAILS))
                .ForMember(dest => dest.EstimatedTime, act => act.MapFrom(src => src.ESTIMATED_TIME))
                .ForMember(dest => dest.Image, act => act.MapFrom(src => src.IMAGE))
                .ForMember(dest => dest.ImageSource, act => act.MapFrom(src => src.IMAGE_SOURCE))
                .ForMember(dest => dest.Link, act => act.MapFrom(src => src.LINK))
                .ForMember(dest => dest.EmpId, act => act.MapFrom(src => src.EMP_ID))
                .ForMember(dest => dest.StatusListId, act => act.MapFrom(src => src.STATUS_LIST_ID))
                .ForMember(dest => dest.ImagePathTofolder, act => act.MapFrom(src => src.IMAGE_PATH))
                .ForMember(dest => dest.ImageId, act => act.MapFrom(src => src.IMAGE_ID))
                ;
        }
    }
}
