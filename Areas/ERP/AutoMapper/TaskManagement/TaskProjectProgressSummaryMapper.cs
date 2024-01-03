using AutoMapper;
using BizWebAPI.Areas.ERP.Dtos.TaskManagement;
using BizWebAPI.Areas.ERP.Models.TaskManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.AutoMapper.TaskManagement
{
    public class TaskProjectProgressSummaryMapper : Profile
    {
        public TaskProjectProgressSummaryMapper()
        {
            CreateMap<TaskProjectProgressSummaryDto, TaskVMDashboard>()
                .ForMember(dest => dest.Complete_Task, act => act.MapFrom(src => src.Complete))
                .ForMember(dest => dest.In_Progress, act => act.MapFrom(src => src.Inprogress))
                .ForMember(dest => dest.Active, act => act.MapFrom(src => src.OpenTask))
                .ForMember(dest => dest.Total_Task, act => act.MapFrom(src => src.Total))
                .ForMember(dest => dest.Remaining_Task, act => act.MapFrom(src => src.Remaining))
                ;


            CreateMap<TaskVMDashboard, TaskProjectProgressSummaryDto>()
                .ForMember(dest => dest.Complete, act => act.MapFrom(src => src.Complete_Task))
                .ForMember(dest => dest.Inprogress, act => act.MapFrom(src => src.In_Progress))
                .ForMember(dest => dest.OpenTask, act => act.MapFrom(src => src.Active))
                 .ForMember(dest => dest.Total, act => act.MapFrom(src => src.Total_Task))
                .ForMember(dest => dest.Remaining, act => act.MapFrom(src => src.Remaining_Task))
                ;
        }
    }
}
