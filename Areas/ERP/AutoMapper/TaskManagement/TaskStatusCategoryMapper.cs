using AutoMapper;
using BizWebAPI.Areas.ERP.Dtos.TaskManagement;
using BizWebAPI.Areas.ERP.Models.TaskManagement;

namespace BizWebAPI.Areas.ERP.AutoMapper.TaskManagement
{
    public class TaskStatusCategoryMapper : Profile
    {
        public TaskStatusCategoryMapper()
        {
            CreateMap<TaskStatusCategoryDto, TaskStatusCategory>()
                .ForMember(dest => dest.STATUS_CATEGORY_ID, act => act.MapFrom(src => src.StatusCategoryId))
                .ForMember(dest => dest.STATUS_NAME, act => act.MapFrom(src => src.StatusName));

            CreateMap<TaskStatusCategory, TaskStatusCategoryDto>()
                .ForMember(dest => dest.StatusCategoryId, act => act.MapFrom(src => src.STATUS_CATEGORY_ID))
                .ForMember(dest => dest.StatusName, act => act.MapFrom(src => src.STATUS_NAME));
        }
    }
}
