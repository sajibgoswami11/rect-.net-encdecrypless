using AutoMapper;
using BizWebAPI.Areas.ERP.Dtos.TaskManagement;
using BizWebAPI.Areas.ERP.Models.TaskManagement;

namespace BizWebAPI.Areas.ERP.AutoMapper.TaskManagement
{
    public class TaskStatusListMapper : Profile
    {
        public TaskStatusListMapper()
        {
            CreateMap<TaskStatusListDto, TaskStatusList>()
                .ForMember(dest => dest.TASK_STATUS_LIST_ID, act => act.MapFrom(src => src.TaskStatusListId))
                .ForMember(dest => dest.STATUS_NAME, act => act.MapFrom(src => src.StatusName))
                .ForMember(dest => dest.PROGRESS_STATUS, act => act.MapFrom(src => src.ProgressStatus))
                .ForMember(dest => dest.STATUS_CATEGORY_ID, act => act.MapFrom(src => src.StatusCategoryId));

            CreateMap<TaskStatusList, TaskStatusListDto>()
                .ForMember(dest => dest.TaskStatusListId, act => act.MapFrom(src => src.TASK_STATUS_LIST_ID))
                .ForMember(dest => dest.StatusName, act => act.MapFrom(src => src.STATUS_NAME))
                .ForMember(dest => dest.ProgressStatus, act => act.MapFrom(src => src.PROGRESS_STATUS))
                .ForMember(dest => dest.StatusCategoryId, act => act.MapFrom(src => src.STATUS_CATEGORY_ID));
        }
    }
}
