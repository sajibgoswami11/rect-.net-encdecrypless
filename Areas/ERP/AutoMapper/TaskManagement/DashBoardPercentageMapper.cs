using AutoMapper;
using BizWebAPI.Areas.ERP.Dtos.TaskManagement;
using BizWebAPI.Areas.ERP.Models.TaskManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.AutoMapper.TaskManagement
{
    public class DashBoardPercentageMapper : Profile
    {
        public DashBoardPercentageMapper()
        {
            CreateMap<DashBoardPercentageDto, TaskVMDashboard>()
                .ForMember(dest => dest.Project_Name, act => act.MapFrom(src => src.ProjectName))
                .ForMember(dest => dest.Project_Id, act => act.MapFrom(src => src.ProjectId))
                .ForMember(dest => dest.Complete_Task, act => act.MapFrom(src => src.Complite))
                .ForMember(dest => dest.Remaining_Task, act => act.MapFrom(src => src.RemainingTask))
                .ForMember(dest => dest.Total_Task, act => act.MapFrom(src => src.TotalTask))

            #region taskdetails by project
                .ForMember(dest => dest.EMP_NAME, act => act.MapFrom(src => src.EmpName))
                .ForMember(dest => dest.TASK_DETAILS, act => act.MapFrom(src => src.TaskDetails))
                .ForMember(dest => dest.ESTIMATED_TIME, act => act.MapFrom(src => src.EstimateTime))
                .ForMember(dest => dest.ASSIGNEE_DATE, act => act.MapFrom(src => src.AssigneeDate))
                .ForMember(dest => dest.TASK_STATUS_LIST_ID, act => act.MapFrom(src => src.ActivityStatus))
                .ForMember(dest => dest.IMAGE, act => act.MapFrom(src => src.TaskImage))
                .ForMember(dest => dest.ACTIVITY_ID, act => act.MapFrom(src => src.ActivityId))
                .ForMember(dest => dest.CREATE_DATE, act => act.MapFrom(src => src.ActivityCreateDate))
                .ForMember(dest => dest.TEMPACTIVITY_TYPE, act => act.MapFrom(src => src.TempActivityType))
                .ForMember(dest => dest.UPDATE_DATE, act => act.MapFrom(src => src.ActivityUpdateDate))
                .ForMember(dest => dest.Duration, act => act.MapFrom(src => src.TaskActivityduration))
                .ForMember(dest => dest.TimeDurationInMinutes, act => act.MapFrom(src => src.TaskActivityDurationInMinutes))
                ;
            #endregion

            CreateMap<TaskVMDashboard, DashBoardPercentageDto>()
                .ForMember(dest => dest.ProjectName, act => act.MapFrom(src => src.Project_Name))
                .ForMember(dest => dest.ProjectId, act => act.MapFrom(src => src.Project_Id))
                .ForMember(dest => dest.Complite, act => act.MapFrom(src => src.Complete_Task))
                .ForMember(dest => dest.RemainingTask, act => act.MapFrom(src => src.Remaining_Task))
                .ForMember(dest => dest.TotalTask, act => act.MapFrom(src => src.Total_Task))

            #region taskdetails by project
                .ForMember(dest => dest.EmpName, act => act.MapFrom(src => src.EMP_NAME))
                .ForMember(dest => dest.TaskDetails, act => act.MapFrom(src => src.TASK_DETAILS))
                .ForMember(dest => dest.EstimateTime, act => act.MapFrom(src => src.ESTIMATED_TIME))
                .ForMember(dest => dest.AssigneeDate, act => act.MapFrom(src => src.ASSIGNEE_DATE))
                .ForMember(dest => dest.ActivityStatus, act => act.MapFrom(src => src.TASK_STATUS_LIST_ID))
                .ForMember(dest => dest.TaskImage, act => act.MapFrom(src => src.IMAGE))
                .ForMember(dest => dest.ActivityId, act => act.MapFrom(src => src.ACTIVITY_ID))
                .ForMember(dest => dest.ActivityCreateDate, act => act.MapFrom(src => src.CREATE_DATE))
                .ForMember(dest => dest.TempActivityType, act => act.MapFrom(src => src.TEMPACTIVITY_TYPE))
                .ForMember(dest => dest.ActivityUpdateDate, act => act.MapFrom(src => src.UPDATE_DATE))
                .ForMember(dest => dest.TaskActivityduration, act => act.MapFrom(src => src.Duration))
                .ForMember(dest => dest.TaskActivityDurationInMinutes, act => act.MapFrom(src => src.TimeDurationInMinutes))

                ;
            #endregion

        }
    }
}
