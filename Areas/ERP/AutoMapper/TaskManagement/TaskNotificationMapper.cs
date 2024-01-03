using AutoMapper;
using BizWebAPI.Areas.ERP.Dtos.TaskManagement;
using BizWebAPI.Areas.ERP.Models.TaskManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.AutoMapper.TaskManagement
{
    public class TaskNotificationMapper : Profile
    {        public TaskNotificationMapper()
        {
            CreateMap<TaskNotificationDto, TaskNotification>()
                .ForMember(dest => dest.NOTIFICATION_ID, act => act.MapFrom(src => src.NotificationId))
                .ForMember(dest => dest.TITLE, act => act.MapFrom(src => src.Title))
                .ForMember(dest => dest.MESSAGE, act => act.MapFrom(src => src.Message))
                .ForMember(dest => dest.SEEN_STATUS, act => act.MapFrom(src => src.SeenStatus))
                .ForMember(dest => dest.SEEN_BY, act => act.MapFrom(src => src.SeenBy))
                .ForMember(dest => dest.LINK, act => act.MapFrom(src => src.Link))
                .ForMember(dest => dest.CREATE_BY, act => act.MapFrom(src => src.CreateBy))
                .ForMember(dest => dest.EXTEND_TIME, act => act.MapFrom(src => src.ExtendTime))
                ;

            CreateMap<TaskNotification, TaskNotificationDto>()
                .ForMember(dest => dest.NotificationId, act => act.MapFrom(src => src.NOTIFICATION_ID))
                .ForMember(dest => dest.Title, act => act.MapFrom(src => src.TITLE))
                .ForMember(dest => dest.Message, act => act.MapFrom(src => src.MESSAGE))
                .ForMember(dest => dest.SeenStatus, act => act.MapFrom(src => src.SEEN_STATUS))
                .ForMember(dest => dest.SeenBy, act => act.MapFrom(src => src.SEEN_BY))
                .ForMember(dest => dest.Link, act => act.MapFrom(src => src.LINK))
                .ForMember(dest => dest.CreateBy, act => act.MapFrom(src => src.CREATE_BY))
                .ForMember(dest => dest.ExtendTime, act => act.MapFrom(src => src.EXTEND_TIME))
                ;
        }
    }
}
