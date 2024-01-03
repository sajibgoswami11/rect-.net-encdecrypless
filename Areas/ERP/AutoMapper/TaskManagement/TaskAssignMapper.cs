using BizWebAPI.Areas.ERP.Dtos.TaskManagement;
using BizWebAPI.Areas.ERP.Models.TaskManagement;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.AutoMapper.TaskManagement
{
    public class TaskAssignMapper : Profile
    {
        public TaskAssignMapper()
        {
            CreateMap<TaskAssignDto, TaskAssign>()
                .ForMember(dest => dest.TASK_ASSIGNEE_ID, act => act.MapFrom(src => src.TaskAssignId))
                .ForMember(dest => dest.TASKLIST_ID, act => act.MapFrom(src => src.TaskListId))
                .ForMember(dest => dest.TASK_DETAILS, act => act.MapFrom(src => src.TaskDetails))
                .ForMember(dest => dest.ASSIGNEE_DATE, act => act.MapFrom(src => src.AssignDate))
                .ForMember(dest => dest.PRIORITY, act => act.MapFrom(src => src.Priority))
                .ForMember(dest => dest.EMP_ID, act => act.MapFrom(src => src.EmployeeId))
                .ForMember(dest => dest.CREATE_BY, act => act.MapFrom(src => src.AssignById))
                .ForMember(dest => dest.EMP_NAME, act => act.MapFrom(src => src.AssignByName))
                .ForMember(dest => dest.IMAGE, act => act.MapFrom(src => src.AssignByImage))
                .ForMember(dest => dest.PROJECT_NAME, act => act.MapFrom(src => src.ProjectName))
                .ForMember(dest => dest.MODULE_NAME, act => act.MapFrom(src => src.ModuleName))
                .ForMember(dest => dest.ESTIMATED_TIME, act => act.MapFrom(src => src.EstimateTime))
                .ForMember(dest => dest.TEMPACTIVITY_TYPE, act => act.MapFrom(src => src.TempActivityType))
                .ForMember(dest => dest.TIME_EXTEND_NOTE, act => act.MapFrom(src => src.TimeExtendNote))
                .ForMember(dest => dest.TASK_IMAGE, act => act.MapFrom(src => src.TaskImage))
                .ForMember(dest => dest.TIME_EXCEED, act => act.MapFrom(src => src.TimeExceed))
                ;


            CreateMap<TaskAssign, TaskAssignDto>()
                .ForMember(dest => dest.TaskAssignId, act => act.MapFrom(src => src.TASK_ASSIGNEE_ID))
                .ForMember(dest => dest.TaskListId, act => act.MapFrom(src => src.TASKLIST_ID))
                .ForMember(dest => dest.TaskDetails, act => act.MapFrom(src => src.TASK_DETAILS))
                .ForMember(dest => dest.AssignDate, act => act.MapFrom(src => src.ASSIGNEE_DATE))
                .ForMember(dest => dest.Priority, act => act.MapFrom(src => src.PRIORITY))
                .ForMember(dest => dest.EmployeeId, act => act.MapFrom(src => src.EMP_ID))
                .ForMember(dest => dest.AssignById, act => act.MapFrom(src => src.CREATE_BY))
                .ForMember(dest => dest.AssignByName, act => act.MapFrom(src => src.EMP_NAME))
                .ForMember(dest => dest.AssignByImage, act => act.MapFrom(src => src.IMAGE))
                 .ForMember(dest => dest.ProjectName, act => act.MapFrom(src => src.PROJECT_NAME))
                .ForMember(dest => dest.ModuleName, act => act.MapFrom(src => src.MODULE_NAME))
                .ForMember(dest => dest.EstimateTime, act => act.MapFrom(src => src.ESTIMATED_TIME))
                .ForMember(dest => dest.TempActivityType , act => act.MapFrom(src => src.TEMPACTIVITY_TYPE))
                .ForMember(dest => dest.TimeExtendNote, act => act.MapFrom(src => src.TIME_EXTEND_NOTE))
                .ForMember(dest => dest.TaskImage, act => act.MapFrom(src => src.TASK_IMAGE))
                .ForMember(dest => dest.TimeExceed, act => act.MapFrom(src => src.TIME_EXCEED))

                ;
        }
    }
}
