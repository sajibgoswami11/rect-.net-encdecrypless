using BizWebAPI.Areas.xEmptyArea.Dtos;
using BizWebAPI.Areas.xEmptyArea.Models;
using AutoMapper;

namespace BizWebAPI.Areas.xEmptyArea.AutoMapper
{
    public class AccountListMapper : Profile
    {
        public AccountListMapper()
        {
            CreateMap<xEmptyAuthDto, AccountList>()
                .ForMember(dest => dest.ACCNT_ID, act => act.MapFrom(src => src.Id))
                .ForMember(dest => dest.USER_ID, act => act.MapFrom(src => src.Username))
                .ForMember(dest => dest.ACCNT_PIN, act => act.MapFrom(src => src.Password));


            CreateMap<AccountList, xEmptyAuthDto>()
                .ForMember(dest => dest.Id, act => act.MapFrom(src => src.ACCNT_ID))
                .ForMember(dest => dest.Username, act => act.MapFrom(src => src.USER_ID))
                .ForMember(dest => dest.Password, act => act.MapFrom(src => src.ACCNT_PIN));
        }
    }
}
