using AutoMapper;
using BizWebAPI.Areas.ERP.Dtos;
using BizWebAPI.Areas.ERP.Dtos.TaskManagement;
using BizWebAPI.Areas.ERP.Interfaces.TaskManagement;
using BizWebAPI.Areas.ERP.Repository;
using BizWebAPI.Areas.ERP.Repository.TaskManagement;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.Manager.TaskManagement
{
    public class TaskUsersManager : ITaskUsers
    {
        private readonly IMapper _mapper;
        private TaskUsersRepository _taskUsersRepository;
        private CommonRepository _commonRepository;
        public TaskUsersManager(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<string> CommonMessage()
        {
            _commonRepository = new CommonRepository();
            return await _commonRepository.CommonMessage();
        }

        public async Task<IEnumerable<TaskUsersDto>> GetUsersList(RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            try
            {
                _taskUsersRepository = new TaskUsersRepository();
                var usersList = await _taskUsersRepository.GetUsersList(requestDetaildto.UserId);
                var usersListMap = _mapper.Map<IEnumerable<TaskUsersDto>>(usersList);
                return usersListMap;
            }
            catch (Exception ex)
            {
                strSysText = ex.Message.ToString();
                throw new Exception(strSysText);
            }
            finally
            {
                _commonRepository = new CommonRepository();
                await _commonRepository.SystemAudit(requestDetaildto, strSysText);
            }
        }

        public async Task<IEnumerable<TaskUsersDto>> GetUsersById(string userId, RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            try
            {
                _commonRepository = new CommonRepository();

                string decryptUserId = await _commonRepository.Decrypt(userId, requestDetaildto.SecurityKey);
                _taskUsersRepository = new TaskUsersRepository();
                var usersList = await _taskUsersRepository.GetUsersById(decryptUserId);
                var usersListMap = _mapper.Map<IEnumerable<TaskUsersDto>>(usersList);
                return usersListMap;
            }
            catch (Exception ex)
            {
                strSysText = ex.Message.ToString();
                throw new Exception(strSysText);
            }
        }

        public async Task<IEnumerable<TaskUsersDto>> GetCompany(RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            try
            {
                _taskUsersRepository = new TaskUsersRepository();
                var usersCompany = await _taskUsersRepository.GetCompany(requestDetaildto.UserId);
                var usersCompanyMap = _mapper.Map<IEnumerable<TaskUsersDto>>(usersCompany);
                return usersCompanyMap;
            }
            catch (Exception ex)
            {
                strSysText = ex.Message.ToString();
                throw new Exception(strSysText);
            }
        }

        public async Task<IEnumerable<TaskUsersDto>> GetCompanyBranch(RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            try
            {
                _taskUsersRepository = new TaskUsersRepository();
                var usersCompany = await _taskUsersRepository.GetCompanyBranch(requestDetaildto.UserId);
                var usersCompanyMap = _mapper.Map<IEnumerable<TaskUsersDto>>(usersCompany);
                return usersCompanyMap;
            }
            catch (Exception ex)
            {
                strSysText = ex.Message.ToString();
                throw new Exception(strSysText);
            }
        }

        public async Task<string> CreateUsers(TaskUsersDto taskUsersDto, RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            string passwordExpiredate = "";
            DateTime decryptpasswordExpiredate = System.DateTime.Today;
            string email = "";
            try
            {
                _commonRepository = new CommonRepository();

                string loginName = await _commonRepository.Decrypt(taskUsersDto.UserLoginName, requestDetaildto.SecurityKey);
                string empId = await _commonRepository.Decrypt(taskUsersDto.EmpId, requestDetaildto.SecurityKey);
                string comId = await _commonRepository.Decrypt(taskUsersDto.CompanyId, requestDetaildto.SecurityKey);
                string groupId = await _commonRepository.Decrypt(taskUsersDto.GroupId, requestDetaildto.SecurityKey);
                string branchId = await _commonRepository.Decrypt(taskUsersDto.BranchId, requestDetaildto.SecurityKey);
                string userPass = await _commonRepository.Decrypt(taskUsersDto.UserPassword, requestDetaildto.SecurityKey);
                if (!string.IsNullOrEmpty(taskUsersDto.PasswordExpiredate))
                {
                    Regex reg = new Regex("[\"]");
                    passwordExpiredate = await _commonRepository.Decrypt(taskUsersDto.PasswordExpiredate, requestDetaildto.SecurityKey);
                    decryptpasswordExpiredate = Convert.ToDateTime(reg.Replace(passwordExpiredate, ""));
                }
                string userType = await _commonRepository.Decrypt(taskUsersDto.UserType, requestDetaildto.SecurityKey);
                if (!string.IsNullOrEmpty(taskUsersDto.UserEmail))
                {
                    email = await _commonRepository.Decrypt(taskUsersDto.UserEmail, requestDetaildto.SecurityKey);
                }
                string userActive = "A";
                _taskUsersRepository = new TaskUsersRepository();
                var usersList = await _taskUsersRepository.CreateUsers(empId, comId, loginName, userPass, userActive, email, groupId, branchId, decryptpasswordExpiredate, userType);
                // var usersListMap = _mapper.Map<IEnumerable<TaskUsersDto>>(usersList);
                return usersList;
            }
            catch (Exception ex)
            {
                strSysText = ex.Message.ToString();
                throw new Exception(strSysText);
            }
            finally
            {
                _commonRepository = new CommonRepository();
                await _commonRepository.SystemAudit(requestDetaildto, strSysText);
            }
        }

        public async Task<string> UpdateUsers(TaskUsersDto taskUsersDto, RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            string passwordExpiredate = "";
            DateTime decryptpasswordExpiredate = System.DateTime.Today;
            string email = "";
            try
            {
                _commonRepository = new CommonRepository();
                string decryptUserId = await _commonRepository.Decrypt(taskUsersDto.UserId, requestDetaildto.SecurityKey);
                string loginName = await _commonRepository.Decrypt(taskUsersDto.UserLoginName, requestDetaildto.SecurityKey);
                string empId = await _commonRepository.Decrypt(taskUsersDto.EmpId, requestDetaildto.SecurityKey);
                string comId = await _commonRepository.Decrypt(taskUsersDto.CompanyId, requestDetaildto.SecurityKey);
                string groupId = await _commonRepository.Decrypt(taskUsersDto.GroupId, requestDetaildto.SecurityKey);
                string branchId = await _commonRepository.Decrypt(taskUsersDto.BranchId, requestDetaildto.SecurityKey);
                string userPass = await _commonRepository.Decrypt(taskUsersDto.UserPassword, requestDetaildto.SecurityKey);
                if (!string.IsNullOrEmpty(taskUsersDto.PasswordExpiredate))
                {
                    Regex reg = new Regex("[\"]");
                    passwordExpiredate = await _commonRepository.Decrypt(taskUsersDto.PasswordExpiredate, requestDetaildto.SecurityKey);
                    decryptpasswordExpiredate = Convert.ToDateTime(reg.Replace(passwordExpiredate, ""));
                }
                string userType = await _commonRepository.Decrypt(taskUsersDto.UserType, requestDetaildto.SecurityKey);
                if (!string.IsNullOrEmpty(taskUsersDto.UserEmail))
                {
                    email = await _commonRepository.Decrypt(taskUsersDto.UserEmail, requestDetaildto.SecurityKey);
                }

                _taskUsersRepository = new TaskUsersRepository();
                var usersList = await _taskUsersRepository.UpdateUsers(empId, decryptUserId, comId, loginName, userPass, "A", email, groupId, branchId, passwordExpiredate, userType);

                return usersList;
            }
            catch (Exception ex)
            {
                strSysText = ex.Message.ToString();
                throw new Exception(strSysText);
            }
            finally
            {
                _commonRepository = new CommonRepository();
                await _commonRepository.SystemAudit(requestDetaildto, strSysText);
            }
        }

        public async Task<IEnumerable<TaskUsersDto>> GetUsersGroupList(RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            try
            {
                _taskUsersRepository = new TaskUsersRepository();
                var usersList = await _taskUsersRepository.GetUserGroup(requestDetaildto.UserId, requestDetaildto.Username);
                var usersListMap = _mapper.Map<IEnumerable<TaskUsersDto>>(usersList);
                return usersListMap;
            }
            catch (Exception ex)
            {
                strSysText = ex.Message.ToString();
                throw new Exception(strSysText);
            }
            finally
            {
                _commonRepository = new CommonRepository();
                await _commonRepository.SystemAudit(requestDetaildto, strSysText);
            }
        }

        public async Task<string> CreateUsersGroup(TaskUsersDto taskUsersDto, RequestDetailDto requestDetailDto)
        {
            string strSysText = "";
            try
            {
                _commonRepository = new CommonRepository();
                string decrypGroupParent = await _commonRepository.Decrypt(taskUsersDto.GroupParent, requestDetailDto.SecurityKey);
                string decrypGroupType = await _commonRepository.Decrypt(taskUsersDto.GroupType, requestDetailDto.SecurityKey);
                string decrypGroupTitle = await _commonRepository.Decrypt(taskUsersDto.GroupTitle, requestDetailDto.SecurityKey);
                string decrypBranchId = await _commonRepository.Decrypt(taskUsersDto.BranchId, requestDetailDto.SecurityKey);

                _taskUsersRepository = new TaskUsersRepository();
                var userGroup = await _taskUsersRepository.CreateUserGroup(decrypGroupParent, decrypGroupTitle, decrypGroupType, decrypBranchId);

                return userGroup;
            }
            catch (Exception ex)
            {
                strSysText = ex.Message.ToString();
                throw new Exception(strSysText);
            }
            finally
            {
                _commonRepository = new CommonRepository();
                await _commonRepository.SystemAudit(requestDetailDto, strSysText);
            }
        }

        public async Task<string> UpdateUsersGroup(TaskUsersDto taskUsersDto, RequestDetailDto requestDetailDto)
        {
            string strSysText = "";
            string decrypBranchId = "";
            try
            {
                _commonRepository = new CommonRepository();
                string decrypGroupId = await _commonRepository.Decrypt(taskUsersDto.GroupId, requestDetailDto.SecurityKey);
                string decrypGroupParent = await _commonRepository.Decrypt(taskUsersDto.GroupParent, requestDetailDto.SecurityKey);
                string decrypGroupType = await _commonRepository.Decrypt(taskUsersDto.GroupType, requestDetailDto.SecurityKey);
                string decrypGroupTitle = await _commonRepository.Decrypt(taskUsersDto.GroupTitle, requestDetailDto.SecurityKey);
                if (!string.IsNullOrEmpty(taskUsersDto.BranchId))
                {
                   decrypBranchId = await _commonRepository.Decrypt(taskUsersDto.BranchId, requestDetailDto.SecurityKey);
                }

                _taskUsersRepository = new TaskUsersRepository();
                var userGroup = await _taskUsersRepository.UpdateUserGroup(decrypGroupId, decrypGroupParent, decrypGroupTitle, decrypGroupType, decrypBranchId);

                return userGroup;
            }
            catch (Exception ex)
            {
                strSysText = ex.Message.ToString();
                throw new Exception(strSysText);
            }
            finally
            {
                _commonRepository = new CommonRepository();
                await _commonRepository.SystemAudit(requestDetailDto, strSysText);
            }
        }

        public async Task<IEnumerable<TaskUsersDto>> GetUsersGroupById(TaskUsersDto taskUsersDto, RequestDetailDto requestDetailDto)
        {
            string strSysText = "";
            try
            {
                _commonRepository = new CommonRepository();
                string decrypGroupId = await _commonRepository.Decrypt(taskUsersDto.GroupId, requestDetailDto.SecurityKey);

                _taskUsersRepository = new TaskUsersRepository();
                var userGroup = await _taskUsersRepository.GetUsersGroupById(decrypGroupId);
                var groupMapData = _mapper.Map<IEnumerable<TaskUsersDto>>(userGroup);
                return groupMapData;
            }
            catch (Exception ex)
            {
                strSysText = ex.Message.ToString();
                throw new Exception(strSysText);
            }
            finally
            {
                _commonRepository = new CommonRepository();
                await _commonRepository.SystemAudit(requestDetailDto, strSysText);
            }
        }

        public async Task<string> ChangePassword(TaskUsersDto taskUsersDto, RequestDetailDto requestDetailDto)
        {
            string strSysText = "";
            string sessionId = "admi0086A2C80001";
            try
            {
                _commonRepository = new CommonRepository();
                string newPassword = await _commonRepository.Decrypt(taskUsersDto.UserPassword, requestDetailDto.SecurityKey);
                string newLoginName = await _commonRepository.Decrypt(taskUsersDto.UserLoginName, requestDetailDto.SecurityKey);

                _taskUsersRepository = new TaskUsersRepository();
                var changePassword = await _taskUsersRepository.DecrypPassword(sessionId, requestDetailDto.Username, requestDetailDto.UserId);
                string oldPassword = changePassword.FirstOrDefault().EXIS_PASS;

                var userPass = await _taskUsersRepository.ChangePassword(requestDetailDto.UserId, newPassword, oldPassword, newLoginName);
                return userPass;
            }
            catch (Exception ex)
            {
                strSysText = ex.Message.ToString();
                throw new Exception(strSysText);
            }
        }

        public async Task<IEnumerable<AccessPolicyDto>> GetCompanyAccessPolicyWiseMenuData(RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            try
            {
                _taskUsersRepository = new TaskUsersRepository();
                var accessPolicyWiseMenuData = await _taskUsersRepository.GetCompanyAccessPolicyWiseMenuData(requestDetaildto.UserId);
                var accessPolicyWiseMenuDataMap = _mapper.Map<IEnumerable<AccessPolicyDto>>(accessPolicyWiseMenuData);
                return accessPolicyWiseMenuDataMap;
            }
            catch (Exception ex)
            {
                strSysText = ex.Message.ToString();
                throw new Exception(strSysText);
            }
            finally
            {
                _commonRepository = new CommonRepository();
                await _commonRepository.SystemAudit(requestDetaildto, strSysText);
            }
        }

        public async Task<IEnumerable<AccessPolicyDto>> GetUserGroupWiseAccessPolicyMenu(RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            try
            {
                _taskUsersRepository = new TaskUsersRepository();
                var accessPolicyWiseMenuData = await _taskUsersRepository.GetUserGroupWiseAccessPolicyMenu(requestDetaildto.UserId);
                var accessPolicyWiseMenuDataMap = _mapper.Map<IEnumerable<AccessPolicyDto>>(accessPolicyWiseMenuData);
                return accessPolicyWiseMenuDataMap;
            }
            catch (Exception ex)
            {
                strSysText = ex.Message.ToString();
                throw new Exception(strSysText);
            }
            finally
            {
                _commonRepository = new CommonRepository();
                await _commonRepository.SystemAudit(requestDetaildto, strSysText);
            }
        }

        public async Task<IEnumerable<AccessPolicyDto>> GetUserWiseAccessPolicy(RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            try
            {
                _taskUsersRepository = new TaskUsersRepository();
                var accessPolicyWiseMenuData = await _taskUsersRepository.GetUserWiseAccessPolicy();
                var accessPolicyWiseMenuDataMap = _mapper.Map<IEnumerable<AccessPolicyDto>>(accessPolicyWiseMenuData);
                return accessPolicyWiseMenuDataMap;
            }
            catch (Exception ex)
            {
                strSysText = ex.Message.ToString();
                throw new Exception(strSysText);
            }
            finally
            {
                _commonRepository = new CommonRepository();
                await _commonRepository.SystemAudit(requestDetaildto, strSysText);
            }
        }

        public async Task<string> CreateRoleWiseAccessPolicyMenu(AccessPolicyDto accessPolicyDto, RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            string roleId = "";
            string userMenuId = "";
            string usrId = "";
            string accessPolicyWiseMenuData = "";
            try
            {
                List<string> decryptMenuList = new List<string>();
                _commonRepository = new CommonRepository();
                foreach (var i in accessPolicyDto.UserMenuList)
                {
                    if (!string.IsNullOrEmpty(i.UserMenuId))
                    {
                        userMenuId = await _commonRepository.Decrypt(i.UserMenuId, requestDetaildto.SecurityKey);
                        decryptMenuList.Add(userMenuId);
                    }
                }
                foreach (var i in accessPolicyDto.UserMenuList)
                {

                    if (!string.IsNullOrEmpty(accessPolicyDto.UserId))
                    { usrId = await _commonRepository.Decrypt(accessPolicyDto.UserId, requestDetaildto.SecurityKey); }
                    if (!string.IsNullOrEmpty(accessPolicyDto.UserGroupId))
                    { roleId = await _commonRepository.Decrypt(accessPolicyDto.UserGroupId, requestDetaildto.SecurityKey); }
                    if (!string.IsNullOrEmpty(i.UserMenuId))
                    {
                        userMenuId = await _commonRepository.Decrypt(i.UserMenuId, requestDetaildto.SecurityKey);
                    }
                    _taskUsersRepository = new TaskUsersRepository();
                    accessPolicyWiseMenuData = await _taskUsersRepository.CreateRoleWiseAccessPolicyMenu(usrId, roleId, userMenuId, decryptMenuList);
                }
                return accessPolicyWiseMenuData;
            }
            catch (Exception ex)
            {
                strSysText = ex.Message.ToString();
                throw new Exception(strSysText);
            }
            finally
            {
                _commonRepository = new CommonRepository();
                await _commonRepository.SystemAudit(requestDetaildto, strSysText);
            }
        }

        public async Task<IEnumerable<AccessPolicyDto>> GetMenuByGroup(AccessPolicyDto accessPolicyDto, RequestDetailDto requestDetailDto)
        {
            string strSysText = "";
            string decrypMenuId = "";
            try
            {
                _commonRepository = new CommonRepository();
                if (!string.IsNullOrEmpty(accessPolicyDto.UserMenuId))
                {
                    decrypMenuId = await _commonRepository.Decrypt(accessPolicyDto.UserMenuId, requestDetailDto.SecurityKey);
                }

                _taskUsersRepository = new TaskUsersRepository();
                var menuData = await _taskUsersRepository.GetMenuByGroup(decrypMenuId, requestDetailDto.UserId);
                var menuDataMap = _mapper.Map<IEnumerable<AccessPolicyDto>>(menuData);
                return menuDataMap;
            }
            catch (Exception ex)
            {
                strSysText = ex.Message.ToString();
                throw new Exception(strSysText);
            }
            finally
            {
                _commonRepository = new CommonRepository();
                await _commonRepository.SystemAudit(requestDetailDto, strSysText);
            }
        }

        public async Task<string> InsertSystemMenu(AccessPolicyDto accessPolicyDto, RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            string parentMenuId = "";
            string menuTitle = "";
            string pathname = "";
            string menuType = "";
            string accessPolicyWiseMenuData = "";
            string menuIcon = "";
            string menuSerial = "";

            try
            {
                _commonRepository = new CommonRepository();

                if (!string.IsNullOrEmpty(accessPolicyDto.ParentMenuId))
                { parentMenuId = await _commonRepository.Decrypt(accessPolicyDto.ParentMenuId, requestDetaildto.SecurityKey); }
                if (!string.IsNullOrEmpty(accessPolicyDto.UserMenuTitle))
                { menuTitle = await _commonRepository.Decrypt(accessPolicyDto.UserMenuTitle, requestDetaildto.SecurityKey); }

                if (!string.IsNullOrEmpty(accessPolicyDto.UserMenuFile))
                { pathname = await _commonRepository.Decrypt(accessPolicyDto.UserMenuFile, requestDetaildto.SecurityKey); }

                menuType = await _commonRepository.Decrypt(accessPolicyDto.MenuType, requestDetaildto.SecurityKey);
                if (!string.IsNullOrEmpty(accessPolicyDto.MenuIcon))
                {
                    menuIcon = await _commonRepository.Decrypt(accessPolicyDto.MenuIcon, requestDetaildto.SecurityKey);
                }
                _taskUsersRepository = new TaskUsersRepository();
                accessPolicyWiseMenuData = await _taskUsersRepository.InsertSystemMenu(requestDetaildto.UserId, parentMenuId, menuTitle, pathname, menuType, menuIcon, menuSerial);


                return accessPolicyWiseMenuData;
            }
            catch (Exception ex)
            {
                strSysText = ex.Message.ToString();
                throw new Exception(strSysText);
            }
            finally
            {
                _commonRepository = new CommonRepository();
                await _commonRepository.SystemAudit(requestDetaildto, strSysText);
            }
        }

        public async Task<IEnumerable<RoleWiseMenuDto>> RoleWiseMenu(UserRoleDto userRole, RequestDetailDto requestDetailDto)
        {
            string strSysText = "";
            try
            {
                _commonRepository = new CommonRepository();
                string decrypMenuId = await _commonRepository.Decrypt(userRole.UserRoleId, requestDetailDto.SecurityKey);

                _taskUsersRepository = new TaskUsersRepository();
                var menuData = await _taskUsersRepository.RoleWiseMenu(decrypMenuId);
                var parentMenuData = menuData.Where(m => m.SYS_MENU_TYPE == "GR").ToList();
                //var childMenuData = menuData.Where(m => m.SYS_MENU_TYPE == "MN").ToList();
                List<RoleWiseMenuDto> parentMenu = new List<RoleWiseMenuDto>();

                foreach (var item in parentMenuData)
                {
                    List<RoleWiseMenuDto> childMenu = new List<RoleWiseMenuDto>();
                    parentMenu.Add(new RoleWiseMenuDto { Text = item.SYS_MENU_TITLE, Value = item.SYS_MENU_ID, Collapsed = true, Children = childMenu });
                    var childMenuData = menuData.Where(itemCh => itemCh.SYS_MENU_PARENT == item.SYS_MENU_ID).ToList();

                    foreach (var child in childMenuData)
                    {
                       
                            List<RoleWiseMenuDto> grandChildMenu = new List<RoleWiseMenuDto>();
                            childMenu.Add(new RoleWiseMenuDto { Text = child.SYS_MENU_TITLE, Value = child.SYS_MENU_ID, Checked = child.IS_ACTIVE.Equals("Y") ? true : false, Collapsed = true, Children = grandChildMenu });
                           
                            var grandChildMenuData = menuData.Where(itemCh => itemCh.SYS_MENU_PARENT == child.SYS_MENU_ID).ToList();

                            foreach (var grand in grandChildMenuData)
                            {
                                if (grand.SYS_MENU_PARENT == child.SYS_MENU_ID)
                                {
                                    List<RoleWiseMenuDto> grandGrandChildMenu = new List<RoleWiseMenuDto>();
                                    grandChildMenu.Add(new RoleWiseMenuDto { Text = grand.SYS_MENU_TITLE, Value = grand.SYS_MENU_ID, Checked = grand.IS_ACTIVE.Equals("Y") ? true : false, Collapsed = true, Children = grandGrandChildMenu });
                                    foreach (var grandGrand in childMenuData)
                                    {
                                        if (grandGrand.SYS_MENU_PARENT == grand.SYS_MENU_ID)
                                        {
                                            grandGrandChildMenu.Add(new RoleWiseMenuDto { Text = grandGrand.SYS_MENU_TITLE, Value = grandGrand.SYS_MENU_ID, Checked = grandGrand.IS_ACTIVE.Equals("Y") ? true : false, Collapsed = true });
                                        }

                                    }
                                }
                            }

                        
                    }
                }

                return parentMenu;
            }
            catch (Exception ex)
            {
                strSysText = ex.Message.ToString();
                throw new Exception(strSysText);
            }
            finally
            {
                _commonRepository = new CommonRepository();
                await _commonRepository.SystemAudit(requestDetailDto, strSysText);
            }
        }

        public async Task<string> PostUserWiseMenu(UserMenuDto userRole, RequestDetailDto requestDetailDto)
        {
            string strSysText = "";
            string decryptMenuId = "";
            try
            {
                List<string> decryptMenuList = new List<string>();
                _commonRepository = new CommonRepository();
                string decrypUserId = await _commonRepository.Decrypt(userRole.UserId, requestDetailDto.SecurityKey);
                foreach (var menu in userRole.MenuId)
                {
                    decryptMenuId = await _commonRepository.Decrypt(menu.UserMenuId, requestDetailDto.SecurityKey);
                    decryptMenuList.Add(decryptMenuId);
                }

                _taskUsersRepository = new TaskUsersRepository();
                var menuData = await _taskUsersRepository.PostUserWiseMenu(decrypUserId, decryptMenuList);

                return null;
            }
            catch (Exception ex)
            {
                strSysText = ex.Message.ToString();
                throw new Exception(strSysText);
            }
            finally
            {
                _commonRepository = new CommonRepository();
                await _commonRepository.SystemAudit(requestDetailDto, strSysText);
            }
        }

        public async Task<string> UpdateSystemMenuById(AccessPolicyDto accessPolicyDto, RequestDetailDto requestDetailDto)
        {
            string strSysText = "";
            string decryptMenuFile = "";
            string decryptMenuTitle = "";
            string menuIcon = "";
            string menuSerial = "";
            try
            {
                _commonRepository = new CommonRepository();
                string decrypMenuId = await _commonRepository.Decrypt(accessPolicyDto.UserMenuId, requestDetailDto.SecurityKey);

                decryptMenuTitle = await _commonRepository.Decrypt(accessPolicyDto.UserMenuTitle, requestDetailDto.SecurityKey);
                if (!string.IsNullOrEmpty(accessPolicyDto.UserMenuFile))
                {
                    decryptMenuFile = await _commonRepository.Decrypt(accessPolicyDto.UserMenuFile, requestDetailDto.SecurityKey);
                }
                if (!string.IsNullOrEmpty(accessPolicyDto.MenuIcon))
                {
                    menuIcon = await _commonRepository.Decrypt(accessPolicyDto.MenuIcon, requestDetailDto.SecurityKey);
                }
                if (!string.IsNullOrEmpty(accessPolicyDto.MenuSerial))
                {
                    menuSerial = await _commonRepository.Decrypt(accessPolicyDto.MenuSerial, requestDetailDto.SecurityKey);
                }

                _taskUsersRepository = new TaskUsersRepository();
                var menuData = await _taskUsersRepository.UpdateSystemMenuById(requestDetailDto.UserId, decrypMenuId, decryptMenuFile, decryptMenuTitle, menuIcon, menuSerial);
                //var menuDataMap = _mapper.Map<IEnumerable<AccessPolicyDto>>(menuData);
                return " Update Success ";
            }
            catch (Exception ex)
            {
                strSysText = ex.Message.ToString();
                throw new Exception(strSysText);
            }
            finally
            {
                _commonRepository = new CommonRepository();
                await _commonRepository.SystemAudit(requestDetailDto, strSysText);
            }

        }

        public async Task<IEnumerable<UserWiseMenuDto>> GetCompanyUserAccessPolicyWiseMenuData(RequestDetailDto requestDetailDto)
        {
            string strSysText = "";
            try
            {
                _taskUsersRepository = new TaskUsersRepository();
                var allMenuData = await _taskUsersRepository.GetCompanyAccessPolicyWiseMenuData(requestDetailDto.UserId);
                var rootParentData = allMenuData.Where(m => m.SYS_MENU_TYPE == "GR").ToList();
                List<UserWiseMenuDto> parentGroup = new List<UserWiseMenuDto>();
                foreach (var itemRoot in rootParentData)
                {
                    List<UserWiseMenuDto> childParentMenu = new List<UserWiseMenuDto>();
                    parentGroup.Add(new UserWiseMenuDto { Name = itemRoot.SYS_MENU_TITLE, Url = itemRoot.PATH_NAME, Children = childParentMenu, Icon = itemRoot.MENU_ICON });

                    var childParentData = allMenuData.Where(item => item.SYS_MENU_PARENT == itemRoot.SYS_MENU_ID).ToList();

                    foreach (var itemChildParent in childParentData)
                    {
                        List<UserWiseMenuDto> childMenu = new List<UserWiseMenuDto>();
                        if (!string.IsNullOrEmpty(itemChildParent.PATH_NAME))
                        {
                            childParentMenu.Add(new UserWiseMenuDto { Name = itemChildParent.SYS_MENU_TITLE, Url = itemChildParent.PATH_NAME, Icon = itemChildParent.MENU_ICON });

                        }
                        else
                        {
                            itemChildParent.PATH_NAME = itemChildParent.SYS_MENU_TITLE;
                            childParentMenu.Add(new UserWiseMenuDto { Name = itemChildParent.SYS_MENU_TITLE, Url = itemChildParent.PATH_NAME, Children = childMenu, Icon = itemChildParent.MENU_ICON });
                        }

                        var childMenuData = allMenuData.Where(item => item.SYS_MENU_PARENT == itemChildParent.SYS_MENU_ID).ToList();

                        foreach (var itemChild in childMenuData)
                        {
                            if (itemChildParent.SYS_MENU_ID == itemChild.SYS_MENU_PARENT)
                            {
                                List<UserWiseMenuDto> grandGrandChildMenu = new List<UserWiseMenuDto>();
                                if (!string.IsNullOrEmpty(itemChildParent.PATH_NAME))
                                {
                                    childMenu.Add(new UserWiseMenuDto { Name = itemChild.SYS_MENU_TITLE, Url = itemChild.PATH_NAME, Icon = itemChild.MENU_ICON });
                                }
                                else
                                {
                                    itemChildParent.PATH_NAME = itemChildParent.SYS_MENU_TITLE;
                                    childMenu.Add(new UserWiseMenuDto { Name = itemChild.SYS_MENU_TITLE, Url = itemChild.PATH_NAME, Children = grandGrandChildMenu, Icon = itemChild.MENU_ICON });

                                }
                            }

                        }
                    }
                }
                return parentGroup;
            }
            catch (Exception ex)
            {
                strSysText = ex.Message.ToString();
                throw new Exception(strSysText);
            }
            finally
            {
                _commonRepository = new CommonRepository();
                await _commonRepository.SystemAudit(requestDetailDto, strSysText);
            }
        }

    }
}
