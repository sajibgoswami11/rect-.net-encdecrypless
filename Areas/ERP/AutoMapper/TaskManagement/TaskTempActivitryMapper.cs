using AutoMapper;
using BizWebAPI.Areas.ERP.Dtos.TaskManagement;
using BizWebAPI.Areas.ERP.Models.TaskManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.AutoMapper.TaskManagement
{
    public class TaskTempActivitryMapper : Profile
    {
        public TaskTempActivitryMapper()
        {
            CreateMap<TaskTempActivitryDto, TaskTempActivitry>()
                .ForMember(dest => dest.TEMPACTIVITY_ID, act => act.MapFrom(src => src.TempActivityId))
                .ForMember(dest => dest.TEMPACTIVITY_TYPE, act => act.MapFrom(src => src.TempActivityType))
                .ForMember(dest => dest.TASK_ASSIGNEE_ID, act => act.MapFrom(src => src.TempActivityId))
                .ForMember(dest => dest.START_TIME, act => act.MapFrom(src => src.StartTime))
                .ForMember(dest => dest.TEMPACTIVITY_TYPE, act => act.MapFrom(src => src.TempActivityType))
                .ForMember(dest => dest.PAUSE_TIME, act => act.MapFrom(src => src.PauseTime));

            CreateMap<TaskTempActivitry, TaskTempActivitryDto>()
                .ForMember(dest => dest.TempActivityId, act => act.MapFrom(src => src.TEMPACTIVITY_ID))
                .ForMember(dest => dest.TempActivityType, act => act.MapFrom(src => src.TEMPACTIVITY_TYPE))
                .ForMember(dest => dest.TaskAssignId, act => act.MapFrom(src => src.TASK_ASSIGNEE_ID))
                .ForMember(dest => dest.StartTime, act => act.MapFrom(src => src.START_TIME))
                .ForMember(dest => dest.TempActivityType, act => act.MapFrom(src => src.TEMPACTIVITY_TYPE))
                .ForMember(dest => dest.PauseTime, act => act.MapFrom(src => src.PAUSE_TIME));
        }        
    }
}
