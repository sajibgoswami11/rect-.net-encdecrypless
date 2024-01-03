using AutoMapper;
using BizWebAPI.Areas.ERP.Dtos.TaskManagement;
using BizWebAPI.Areas.ERP.Models.TaskManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.AutoMapper.TaskManagement
{
    public class AssignTimeExtendMapper : Profile
    {
        public AssignTimeExtendMapper()
        {
            CreateMap<AssignTimeExtendDto, TaskTempActivitry>()
                .ForMember(dest => dest.TASK_ASSIGNEE_ID, act => act.MapFrom(src => src.TaskAssignId))
                .ForMember(dest => dest.TIME_EXTEND_NOTE, act => act.MapFrom(src => src.TimeExtendNote))
                .ForMember(dest => dest.EMP_ID, act => act.MapFrom(src => src.EmpId))
                ;

            CreateMap<TaskTempActivitry, AssignTimeExtendDto>()
                .ForMember(dest => dest.TaskAssignId, act => act.MapFrom(src => src.TASK_ASSIGNEE_ID))
                .ForMember(dest => dest.TimeExtendNote, act => act.MapFrom(src => src.TIME_EXTEND_NOTE))
                .ForMember(dest => dest.EmpId, act => act.MapFrom(src => src.EMP_ID))
                ;

        }
    }
}
