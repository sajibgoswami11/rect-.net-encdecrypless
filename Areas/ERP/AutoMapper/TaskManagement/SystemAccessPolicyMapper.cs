using AutoMapper;
using BizWebAPI.Areas.ERP.Dtos.TaskManagement;
using BizWebAPI.Areas.ERP.Models.TaskManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.AutoMapper.TaskManagement
{
    public class SystemAccessPolicyMapper : Profile
    {
        public SystemAccessPolicyMapper()
        {
            CreateMap<AccessPolicyDto, CmSystemAccessPolicy>()
                    .ForMember(dest => dest.SYS_USR_GRP_ID, act => act.MapFrom(src => src.UserGroupId))
                    .ForMember(dest => dest.SYS_MENU_ID, act => act.MapFrom(src => src.UserMenuId))
                    .ForMember(dest => dest.SYS_MENU_TITLE, act => act.MapFrom(src => src.UserMenuTitle))
                    .ForMember(dest => dest.PATH_NAME, act => act.MapFrom(src => src.UserMenuFile))
                    .ForMember(dest => dest.SYS_USR_GRP_TITLE, act => act.MapFrom(src => src.UserGroupTitle))
                    .ForMember(dest => dest.SYS_USR_ID, act => act.MapFrom(src => src.UserId))
                    .ForMember(dest => dest.SYS_USR_LOGIN_NAME, act => act.MapFrom(src => src.UserName))
                    .ForMember(dest => dest.SYS_MENU_PARENT, act => act.MapFrom(src => src.ParentMenuId))
                    .ForMember(dest => dest.SYS_MENU_TYPE, act => act.MapFrom(src => src.MenuType))
                    .ForMember(dest => dest.MENU_ICON, act => act.MapFrom(src => src.MenuIcon))
                    .ForMember(dest => dest.SYS_MENU_SERIAL, act => act.MapFrom(src => src.MenuSerial))
            ;

            CreateMap<CmSystemAccessPolicy, AccessPolicyDto>()
                   .ForMember(dest => dest.UserGroupId, act => act.MapFrom(src => src.SYS_USR_GRP_ID))
                   .ForMember(dest => dest.UserMenuId, act => act.MapFrom(src => src.SYS_MENU_ID))
                   .ForMember(dest => dest.UserMenuFile, act => act.MapFrom(src => src.PATH_NAME))
                   .ForMember(dest => dest.UserMenuTitle, act => act.MapFrom(src => src.SYS_MENU_TITLE))
                   .ForMember(dest => dest.UserGroupTitle, act => act.MapFrom(src => src.SYS_USR_GRP_TITLE))
                   .ForMember(dest => dest.UserId, act => act.MapFrom(src => src.SYS_USR_ID))
                   .ForMember(dest => dest.UserName, act => act.MapFrom(src => src.SYS_USR_LOGIN_NAME))
                   .ForMember(dest => dest.ParentMenuId, act => act.MapFrom(src => src.SYS_MENU_PARENT))
                   .ForMember(dest => dest.MenuType, act => act.MapFrom(src => src.SYS_MENU_TYPE))
                   .ForMember(dest => dest.MenuIcon, act => act.MapFrom(src => src.MENU_ICON))
                    .ForMember(dest => dest.MenuSerial, act => act.MapFrom(src => src.SYS_MENU_SERIAL))

                ;

        }



    }
}
