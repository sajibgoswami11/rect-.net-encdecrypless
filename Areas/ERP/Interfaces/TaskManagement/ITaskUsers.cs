using BizWebAPI.Areas.ERP.Dtos;
using BizWebAPI.Areas.ERP.Dtos.TaskManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.Interfaces.TaskManagement
{
    public interface ITaskUsers
    {
        Task<string> CommonMessage();
        Task<IEnumerable<TaskUsersDto>> GetUsersList(RequestDetailDto requestDetaildto);
        Task<IEnumerable<TaskUsersDto>> GetUsersGroupList(RequestDetailDto requestDetaildto);
        Task<string> CreateUsersGroup(TaskUsersDto taskUsersDto, RequestDetailDto requestDetaildto);
        Task<string> UpdateUsersGroup(TaskUsersDto taskUsersDto, RequestDetailDto requestDetailDto);
        Task<IEnumerable<TaskUsersDto>> GetUsersGroupById(TaskUsersDto taskUsersDto, RequestDetailDto requestDetailDto);
        Task<string> CreateUsers(TaskUsersDto taskUsersDto, RequestDetailDto requestDetaildto);
        Task<string> UpdateUsers(TaskUsersDto taskUsersDto, RequestDetailDto requestDetaildto);
        Task<string> ChangePassword(TaskUsersDto taskUsersDto, RequestDetailDto requestDetailDto);
        Task<IEnumerable<AccessPolicyDto>> GetCompanyAccessPolicyWiseMenuData(RequestDetailDto requestDetailDto);

        Task<IEnumerable<AccessPolicyDto>> GetUserGroupWiseAccessPolicyMenu(RequestDetailDto requestDetailDto);
        Task<IEnumerable<AccessPolicyDto>> GetUserWiseAccessPolicy(RequestDetailDto requestDetaildto);
        Task<string> CreateRoleWiseAccessPolicyMenu(AccessPolicyDto accessPolicyDto, RequestDetailDto requestDetaildto);

        Task<IEnumerable<AccessPolicyDto>> GetMenuByGroup(AccessPolicyDto accessPolicyDto, RequestDetailDto requestDetailDto);
        Task<string> InsertSystemMenu(AccessPolicyDto accessPolicyDto, RequestDetailDto requestDetaildto);

        Task<IEnumerable<RoleWiseMenuDto>> RoleWiseMenu(UserRoleDto userRole, RequestDetailDto requestDetailDto);
        Task<string> PostUserWiseMenu(UserMenuDto userMenu, RequestDetailDto requestDetailDto);
        Task<string> UpdateSystemMenuById(AccessPolicyDto accessPolicyDto, RequestDetailDto requestDetailDto);
        Task<IEnumerable<TaskUsersDto>> GetUsersById(string userId, RequestDetailDto requestDetaildto);

        Task<IEnumerable<TaskUsersDto>> GetCompanyBranch(RequestDetailDto requestDetaildto);
        Task<IEnumerable<TaskUsersDto>> GetCompany(RequestDetailDto requestDetaildto);
        Task<IEnumerable<UserWiseMenuDto>> GetCompanyUserAccessPolicyWiseMenuData(RequestDetailDto requestDetailDto);

    }
}
