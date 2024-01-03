using AutoMapper;
using BizWebAPI.Areas.ERP.Dtos.TaskManagement;
using BizWebAPI.Areas.ERP.Models.TaskManagement;

namespace BizWebAPI.Areas.ERP.AutoMapper.TaskManagement
{
    public class PrEmployeeListMapper : Profile
    {
        public PrEmployeeListMapper()
        {
            CreateMap<PrEmployeeListDto, PrEmployeeList>()
                .ForMember(dest => dest.EMP_NAME, act => act.MapFrom(src => src.EmpName))
                .ForMember(dest => dest.EMP_ID, act => act.MapFrom(src => src.EmpId))
                .ForMember(dest => dest.EMP_TITLE, act => act.MapFrom(src => src.EmpTitle))
                .ForMember(dest => dest.EMP_EMAIL, act => act.MapFrom(src => src.EmpEmail))
                .ForMember(dest => dest.EMP_CODE, act => act.MapFrom(src => src.EmpCode))
                .ForMember(dest => dest.CMP_BRANCH_ID, act => act.MapFrom(src => src.CmpBranchId))
                .ForMember(dest => dest.SYS_USR_ID, act => act.MapFrom(src => src.SysUserId))
                .ForMember(dest => dest.EMP_CONTACT_NUM, act => act.MapFrom(src => src.ContactNumber))
                .ForMember(dest => dest.EMP_PRE_ADDRES, act => act.MapFrom(src => src.PresentAddress))
                .ForMember(dest => dest.EMP_PER_ADDRESS, act => act.MapFrom(src => src.PermanentAddress))
                .ForMember(dest => dest.EMP_PIC, act => act.MapFrom(src => src.ImagePath))
                .ForMember(dest => dest.EMP_NATIONAL_ID, act => act.MapFrom(src => src.NidCard))
                .ForMember(dest => dest.DPT_ID, act => act.MapFrom(src => src.DepartmentId))
                .ForMember(dest => dest.DSG_ID, act => act.MapFrom(src => src.DesignationId))
                .ForMember(dest => dest.EMP_BANK_ACC_NO, act => act.MapFrom(src => src.AccountNo))
                .ForMember(dest => dest.EMP_JOINING_DATE, act => act.MapFrom(src => src.JoiningDate))
                .ForMember(dest => dest.EMP_BIRTHDAY, act => act.MapFrom(src => src.BirthDate))
                ;

            CreateMap<PrEmployeeList, PrEmployeeListDto>()
                .ForMember(dest => dest.EmpName, act => act.MapFrom(src => src.EMP_NAME))
                .ForMember(dest => dest.EmpId, act => act.MapFrom(src => src.EMP_ID))
                .ForMember(dest => dest.EmpTitle, act => act.MapFrom(src => src.EMP_TITLE))
                .ForMember(dest => dest.EmpEmail, act => act.MapFrom(src => src.EMP_EMAIL))
                .ForMember(dest => dest.EmpCode, act => act.MapFrom(src => src.EMP_CODE))
                .ForMember(dest => dest.SysUserId, act => act.MapFrom(src => src.SYS_USR_ID))
                .ForMember(dest => dest.CmpBranchId, act => act.MapFrom(src => src.CMP_BRANCH_ID))
                .ForMember(dest => dest.ContactNumber, act => act.MapFrom(src => src.EMP_CONTACT_NUM))
                .ForMember(dest => dest.PresentAddress, act => act.MapFrom(src => src.EMP_PRE_ADDRES))
                .ForMember(dest => dest.PermanentAddress, act => act.MapFrom(src => src.EMP_PER_ADDRESS))
                .ForMember(dest => dest.ImagePath, act => act.MapFrom(src => src.EMP_PIC))
                .ForMember(dest => dest.NidCard, act => act.MapFrom(src => src.EMP_NATIONAL_ID))
                .ForMember(dest => dest.DepartmentId, act => act.MapFrom(src => src.DPT_ID))
                .ForMember(dest => dest.DesignationId, act => act.MapFrom(src => src.DSG_ID))
                .ForMember(dest => dest.AccountNo, act => act.MapFrom(src => src.EMP_BANK_ACC_NO))
                .ForMember(dest => dest.JoiningDate, act => act.MapFrom(src => src.EMP_JOINING_DATE))
                .ForMember(dest => dest.BirthDate, act => act.MapFrom(src => src.EMP_BIRTHDAY))
                ;
        }
    }
}
