using AutoMapper;
using BizWebAPI.Areas.ERP.Dtos.TaskManagement;
using BizWebAPI.Areas.ERP.Models.TaskManagement;

namespace BizWebAPI.Areas.ERP.AutoMapper.TaskManagement
{
    public class TaskForwardMapper : Profile
    {
        public TaskForwardMapper()
        {
            CreateMap<TaskForwardDto, TaskForward>()
                .ForMember(dest => dest.TASK_FORWADR_ID, act => act.MapFrom(src => src.TaskForwardId))
                .ForMember(dest => dest.TASK_ASSIGNEE_ID, act => act.MapFrom(src => src.TaskAssigneeId))
                .ForMember(dest => dest.EMP_ID, act => act.MapFrom(src => src.EmpId))
                .ForMember(dest => dest.TASK_DETAILS, act => act.MapFrom(src => src.TaskDetails))
                .ForMember(dest => dest.TASK_FORWARD_DATE, act => act.MapFrom(src => src.TaskForwardDate))
                .ForMember(dest => dest.PRIORITY, act => act.MapFrom(src => src.Priority));

            CreateMap<TaskForward, TaskForwardDto>()
                .ForMember(dest => dest.TaskForwardId, act => act.MapFrom(src => src.TASK_FORWADR_ID))
                .ForMember(dest => dest.TaskAssigneeId, act => act.MapFrom(src => src.TASK_ASSIGNEE_ID))
                .ForMember(dest => dest.EmpId, act => act.MapFrom(src => src.EMP_ID))
                .ForMember(dest => dest.TaskDetails, act => act.MapFrom(src => src.TASK_DETAILS))
                .ForMember(dest => dest.TaskForwardDate, act => act.MapFrom(src => src.TASK_FORWARD_DATE))
                .ForMember(dest => dest.Priority, act => act.MapFrom(src => src.PRIORITY));
        }
    }
}
