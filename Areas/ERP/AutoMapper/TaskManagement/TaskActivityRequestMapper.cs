using AutoMapper;
using BizWebAPI.Areas.ERP.Dtos.TaskManagement;
using BizWebAPI.Areas.ERP.Models.TaskManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.AutoMapper.TaskManagement
{
    public class TaskActivityRequestMapper : Profile
    {
        public TaskActivityRequestMapper()
        {
            CreateMap<TaskActivityRequestDto, TaskActivity>()
                .ForMember(dest => dest.ACTIVITY_ID, act => act.MapFrom(src => src.ActivityId))
                .ForMember(dest => dest.START_TIME, act => act.MapFrom(src => src.StartTime))
                .ForMember(dest => dest.END_TIME, act => act.MapFrom(src => src.EndTime))
                .ForMember(dest => dest.REMARKS, act => act.MapFrom(src => src.Remarks))
                .ForMember(dest => dest.TASK_STATUS_LIST_ID, act => act.MapFrom(src => src.TaskStatusListId))
                .ForMember(dest => dest.EMP_ID, act => act.MapFrom(src => src.EmpId))
                .ForMember(dest => dest.TASK_ASSIGNEE_ID, act => act.MapFrom(src => src.TaskAssigneeId))
                .ForMember(dest => dest.TASK_ACTIVITY_DATE, act => act.MapFrom(src => src.TaskactivityDate))
                .ForMember(dest => dest.ACTIVITY_IMAGE, act => act.MapFrom(src => src.ActivityImage))
                .ForMember(dest => dest.ACTIVITY_IMAGE_SOURCE, act => act.MapFrom(src => src.ActivityImageSource));

            CreateMap<TaskActivity, TaskActivityRequestDto>()
                .ForMember(dest => dest.ActivityId, act => act.MapFrom(src => src.ACTIVITY_ID))
                .ForMember(dest => dest.StartTime, act => act.MapFrom(src => src.START_TIME))
                .ForMember(dest => dest.EndTime, act => act.MapFrom(src => src.END_TIME))
                .ForMember(dest => dest.Remarks, act => act.MapFrom(src => src.REMARKS))
                .ForMember(dest => dest.TaskStatusListId, act => act.MapFrom(src => src.TASK_STATUS_LIST_ID))
                .ForMember(dest => dest.EmpId, act => act.MapFrom(src => src.EMP_ID))
                .ForMember(dest => dest.TaskAssigneeId, act => act.MapFrom(src => src.TASK_ASSIGNEE_ID))
                .ForMember(dest => dest.TaskactivityDate, act => act.MapFrom(src => src.TASK_ACTIVITY_DATE))
                .ForMember(dest => dest.ActivityImage, act => act.MapFrom(src => src.ACTIVITY_IMAGE));
        }
    }
}
