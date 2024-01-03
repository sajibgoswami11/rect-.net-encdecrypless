using AutoMapper;
using BizWebAPI.Areas.ERP.Dtos;
using BizWebAPI.Areas.ERP.Dtos.TaskManagement;
using BizWebAPI.Areas.ERP.Models;
using BizWebAPI.Areas.ERP.Models.TaskManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.AutoMapper.TaskManagement
{
    public class TaskTeamMemberMapper : Profile
    {
        public TaskTeamMemberMapper()
        {
            CreateMap<TaskEmpTeamMappingDto, TasVMkEmpTeamMapping>()
                 .ForMember(dest => dest.TEAM_ID, act => act.MapFrom(src => src.TeamId))
                 .ForMember(dest => dest.EMP_NAME, act => act.MapFrom(src => src.EmpName))
                 .ForMember(dest => dest.EMP_ID, act => act.MapFrom(src => src.EmpId))
                 .ForMember(dest => dest.IMAGE, act => act.MapFrom(src => src.EmpImage));

            CreateMap<TasVMkEmpTeamMapping, TaskEmpTeamMappingDto>()
                .ForMember(dest => dest.TeamId, act => act.MapFrom(src => src.TEAM_ID))
                .ForMember(dest => dest.EmpName, act => act.MapFrom(src => src.EMP_NAME))
                .ForMember(dest => dest.EmpId, act => act.MapFrom(src => src.EMP_ID))
                .ForMember(dest => dest.EmpImage, act => act.MapFrom(src => src.IMAGE));
        }
    }
}
