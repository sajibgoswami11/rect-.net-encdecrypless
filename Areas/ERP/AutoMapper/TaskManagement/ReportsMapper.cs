using AutoMapper;
using BizWebAPI.Areas.ERP.Dtos.TaskManagement;
using BizWebAPI.Areas.ERP.Models.TaskManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.AutoMapper.TaskManagement
{
    public class ReportsMapper : Profile
    {
        public ReportsMapper()
        {
            CreateMap<TaskReportDto, TaskReport>()
                .ForMember(dest => dest.TASKLIST_ID, act => act.MapFrom(src => src.TaskListId))
                .ForMember(dest => dest.MODULE_NAME, act => act.MapFrom(src => src.ModuleName))
                .ForMember(dest => dest.PROJECT_NAME, act => act.MapFrom(src => src.ProjectName))
                .ForMember(dest => dest.TASK_DETAILS, act => act.MapFrom(src => src.TaskDetails))
                .ForMember(dest => dest.ACTIVITY_STATUS, act => act.MapFrom(src => src.ActivityStatus))
                .ForMember(dest => dest.IMAGE, act => act.MapFrom(src => src.Image))
                .ForMember(dest => dest.LINK, act => act.MapFrom(src => src.Link))
                .ForMember(dest => dest.TEMPACTIVITY_TYPE, act => act.MapFrom(src => src.OnWork))
                .ForMember(dest => dest.TIME_EXTEND_NOTE, act => act.MapFrom(src => src.TimeExtendNote))
                .ForMember(dest => dest.REMARKS, act => act.MapFrom(src => src.TaskRemarks))
                .ForMember(dest => dest.DURATION_OF, act => act.MapFrom(src => src.Duration))
                .ForMember(dest => dest.START_TIME, act => act.MapFrom(src => src.StartTime))
                .ForMember(dest => dest.END_TIME, act => act.MapFrom(src => src.EndTime))
                .ForMember(dest => dest.EMP_NAME, act => act.MapFrom(src => src.EmpName))
                .ForMember(dest => dest.TASKCREATOR_NAME, act => act.MapFrom(src => src.TaskCreatorName))
                .ForMember(dest => dest.ASSIGNEE_DATE, act => act.MapFrom(src => src.AssignDate))
                .ForMember(dest => dest.EMP_ID, act => act.MapFrom(src => src.EmpId))
                .ForMember(dest => dest.ESTIMATED_TIME, act => act.MapFrom(src => src.EstimateTime))
                .ForMember(dest => dest.TASK_DATE, act => act.MapFrom(src => src.TaskDate))
                .ForMember(dest => dest.EXTEND_TIME, act => act.MapFrom(src => src.ExtendTime))
                .ForMember(dest => dest.DURATION_HOUR, act => act.MapFrom(src => src.TimeDuration))
                .ForMember(dest => dest.DURATIONINMINUTES, act => act.MapFrom(src => src.TimeDurationInMinutes))
                ;

            CreateMap<TaskReport, TaskReportDto>()
                .ForMember(dest => dest.TaskListId, act => act.MapFrom(src => src.TASKLIST_ID))
                .ForMember(dest => dest.ModuleName, act => act.MapFrom(src => src.MODULE_NAME))
                .ForMember(dest => dest.ProjectName, act => act.MapFrom(src => src.PROJECT_NAME))
                .ForMember(dest => dest.TaskDetails, act => act.MapFrom(src => src.TASK_DETAILS))
                .ForMember(dest => dest.ActivityStatus, act => act.MapFrom(src => src.ACTIVITY_STATUS))
                .ForMember(dest => dest.Image, act => act.MapFrom(src => src.IMAGE))
                .ForMember(dest => dest.Link, act => act.MapFrom(src => src.LINK))
                .ForMember(dest => dest.OnWork, act => act.MapFrom(src => src.TEMPACTIVITY_TYPE))
                .ForMember(dest => dest.TimeExtendNote, act => act.MapFrom(src => src.TIME_EXTEND_NOTE))
                .ForMember(dest => dest.TaskRemarks, act => act.MapFrom(src => src.REMARKS))
                .ForMember(dest => dest.Duration, act => act.MapFrom(src => src.DURATION_OF))
                .ForMember(dest => dest.StartTime, act => act.MapFrom(src => src.START_TIME))
                .ForMember(dest => dest.EndTime, act => act.MapFrom(src => src.END_TIME))
                .ForMember(dest => dest.EmpName, act => act.MapFrom(src => src.EMP_NAME))
                .ForMember(dest => dest.TaskCreatorName, act => act.MapFrom(src => src.TASKCREATOR_NAME))
                .ForMember(dest => dest.AssignDate, act => act.MapFrom(src => src.ASSIGNEE_DATE))
                .ForMember(dest => dest.EmpId, act => act.MapFrom(src => src.EMP_ID))
                .ForMember(dest => dest.EstimateTime, act => act.MapFrom(src => src.ESTIMATED_TIME))
                .ForMember(dest => dest.TaskDate, act => act.MapFrom(src => src.TASK_DATE))
                .ForMember(dest => dest.ExtendTime, act => act.MapFrom(src => src.EXTEND_TIME))
                .ForMember(dest => dest.TimeDuration, act => act.MapFrom(src => src.DURATION_HOUR))
                .ForMember(dest => dest.TimeDurationInMinutes, act => act.MapFrom(src => src.DURATIONINMINUTES))
                ;
        }
    }
}
