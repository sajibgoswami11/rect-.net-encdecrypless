using BizWebAPI.Areas.ERP.Dtos;
using BizWebAPI.Areas.ERP.Models;
using AutoMapper;

namespace BizWebAPI.Areas.ERP.AutoMapper
{
    public class SystemUserMapper : Profile
    {
        public SystemUserMapper()
        {
            CreateMap<ErpAuthLoginDto, SystemUser>()
                .ForMember(dest => dest.SYS_USR_ID, act => act.MapFrom(src => src.UserId))
                .ForMember(dest => dest.SYS_USR_LOGIN_NAME, act => act.MapFrom(src => src.Username))
                .ForMember(dest => dest.SYS_USR_PASS, act => act.MapFrom(src => src.Password))
                .ForMember(dest => dest.SYS_USR_GRP_TITLE, act => act.MapFrom(src => src.UserGroup))
                .ForMember(dest => dest.EMP_ID, act => act.MapFrom(src => src.EmpId))
                .ForMember(dest => dest.EMP_TITLE, act => act.MapFrom(src => src.Title))
                .ForMember(dest => dest.EMP_NAME, act => act.MapFrom(src => src.Name))
                .ForMember(dest => dest.EMP_PIC, act => act.MapFrom(src => src.Image))
                .ForMember(dest => dest.EMP_EMAIL, act => act.MapFrom(src => src.Email));

            CreateMap<SystemUser, ErpAuthLoginDto>()
                .ForMember(dest => dest.UserId, act => act.MapFrom(src => src.SYS_USR_ID))
                .ForMember(dest => dest.Username, act => act.MapFrom(src => src.SYS_USR_LOGIN_NAME))
                .ForMember(dest => dest.Password, act => act.MapFrom(src => src.SYS_USR_PASS))
                .ForMember(dest => dest.UserGroup, act => act.MapFrom(src => src.SYS_USR_GRP_TITLE))
                .ForMember(dest => dest.EmpId, act => act.MapFrom(src => src.EMP_ID))
                .ForMember(dest => dest.Title, act => act.MapFrom(src => src.EMP_TITLE))
                .ForMember(dest => dest.Name, act => act.MapFrom(src => src.EMP_NAME))
                .ForMember(dest => dest.Image, act => act.MapFrom(src => src.EMP_PIC))
                .ForMember(dest => dest.Email, act => act.MapFrom(src => src.EMP_EMAIL));
        }
    }
}
