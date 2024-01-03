using BizWebAPI.Areas.ERP.Dtos;
using BizWebAPI.Areas.ERP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BizWebAPI.Areas.ERP.Models.TaskManagement;

namespace BizWebAPI.Areas.ERP.AutoMapper
{
    public class SystemUserEmployeeMapper : Profile
    {
        public SystemUserEmployeeMapper()
        {
            CreateMap<ErpAuthLoginReturnDto, PrEmployeeList>()
                .ForMember(dest => dest.EMP_ID, act => act.MapFrom(src => src.UserId))
                .ForMember(dest => dest.EMP_TITLE, act => act.MapFrom(src => src.Title))
                .ForMember(dest => dest.EMP_NAME, act => act.MapFrom(src => src.Name))
                .ForMember(dest => dest.EMP_PIC, act => act.MapFrom(src => src.Image))
                .ForMember(dest => dest.EMP_EMAIL, act => act.MapFrom(src => src.Email));

            CreateMap<PrEmployeeList, ErpAuthLoginReturnDto>()
                .ForMember(dest => dest.UserId, act => act.MapFrom(src => src.EMP_ID))
                .ForMember(dest => dest.Title, act => act.MapFrom(src => src.EMP_TITLE))
                .ForMember(dest => dest.Name, act => act.MapFrom(src => src.EMP_NAME))
                .ForMember(dest => dest.Image, act => act.MapFrom(src => src.EMP_PIC))
                .ForMember(dest => dest.Email, act => act.MapFrom(src => src.EMP_EMAIL));
        }
    }
}
