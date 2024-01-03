using AutoMapper;
using BizWebAPI.Areas.ERP.Dtos.TaskManagement;
using BizWebAPI.Areas.ERP.Models.TaskManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.AutoMapper.TaskManagement
{
    public class TaskUsersMapper :Profile
    {
        public TaskUsersMapper()
        {
            CreateMap<TaskUsersDto, CmSystemUsers>()
                .ForMember(dest => dest.SYS_USR_ID, act => act.MapFrom(src => src.UserId))
                .ForMember(dest => dest.SYS_USR_GRP_ID, act => act.MapFrom(src => src.GroupId))
                .ForMember(dest => dest.SYS_USR_DNAME, act => act.MapFrom(src => src.UserName))
                .ForMember(dest => dest.SYS_USR_LOGIN_NAME, act => act.MapFrom(src => src.UserLoginName))
                .ForMember(dest => dest.SYS_USR_GRP_TITLE, act => act.MapFrom(src => src.GroupTitle))
                .ForMember(dest => dest.CMP_BRANCH_ID, act => act.MapFrom(src => src.BranchId))
                .ForMember(dest => dest.SYS_USR_GRP_TYPE, act => act.MapFrom(src => src.GroupType))
                .ForMember(dest => dest.SYS_USR_GRP_PARENT, act => act.MapFrom(src => src.GroupParent))
                .ForMember(dest => dest.SYS_USR_PASS, act => act.MapFrom(src => src.UserPassword))
                .ForMember(dest => dest.COMPANY_ID, act => act.MapFrom(src => src.CompanyId))
                .ForMember(dest => dest.CMP_BRANCH_NAME, act => act.MapFrom(src => src.BranchName))
                .ForMember(dest => dest.COMPANY_NAME, act => act.MapFrom(src => src.CompanyName))
                .ForMember(dest => dest.USER_TYPE, act => act.MapFrom(src => src.UserType))
                ;

            CreateMap<CmSystemUsers, TaskUsersDto>()
                .ForMember(dest => dest.UserId, act => act.MapFrom(src => src.SYS_USR_ID))
                .ForMember(dest => dest.GroupId, act => act.MapFrom(src => src.SYS_USR_GRP_ID))
                .ForMember(dest => dest.UserName, act => act.MapFrom(src => src.SYS_USR_DNAME))
                .ForMember(dest => dest.UserLoginName, act => act.MapFrom(src => src.SYS_USR_LOGIN_NAME))
                .ForMember(dest => dest.GroupTitle, act => act.MapFrom(src => src.SYS_USR_GRP_TITLE))
                 .ForMember(dest => dest.BranchId, act => act.MapFrom(src => src.CMP_BRANCH_ID))
                .ForMember(dest => dest.GroupType, act => act.MapFrom(src => src.SYS_USR_GRP_TYPE))
                .ForMember(dest => dest.GroupParent, act => act.MapFrom(src => src.SYS_USR_GRP_PARENT))
                .ForMember(dest => dest.UserPassword, act => act.MapFrom(src => src.SYS_USR_PASS))
                .ForMember(dest => dest.CompanyId, act => act.MapFrom(src => src.COMPANY_ID))
                .ForMember(dest => dest.CompanyName, act => act.MapFrom(src => src.COMPANY_NAME))
                .ForMember(dest => dest.UserType, act => act.MapFrom(src => src.USER_TYPE))
                .ForMember(dest => dest.BranchName, act => act.MapFrom(src => src.CMP_BRANCH_NAME))
                ;
        }
    }
}
