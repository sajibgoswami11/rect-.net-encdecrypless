using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BizWebAPI.Areas.ERP.Dtos.TaskManagement;
using BizWebAPI.Areas.ERP.Interfaces.TaskManagement;
using BizWebAPI.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;

namespace BizWebAPI.Areas.ERP.Controllers.TaskManagement
{
    [Authorize]
    [Area("ERP")]
    [Route("ERP/TaskManagement/[controller]")]
    [ApiController]
    public class TaskUsersController : ControllerBase
    {
        private readonly ITaskUsers _users;

        public TaskUsersController(ITaskUsers users)
        {
            _users = users;
        }

        [HttpGet("GetUsersList")]
        public async Task<IActionResult> GetUsersList()
        {
            var userDetails = await GetKey.GetSecurityKey(HttpContext);
            var usersList = await _users.GetUsersList(userDetails);
            if (usersList != null)
            {
                return Ok(new { Message = "Data Found", usersList, Data = true });
            }
            else
            {
                return Ok(new { Message = "Data Not Found", usersList, Data = false });
            }
        }
        [HttpGet("GetCompanyBranch")]
        public async Task<IActionResult> GetCompanyBranch()
        {
            var userDetails = await GetKey.GetSecurityKey(HttpContext);
            var usersBranch = await _users.GetCompanyBranch(userDetails);
            if (usersBranch != null)
            {
                return Ok(new { Message = "Data Found", usersBranch, Data = true });
            }
            else
            {
                return Ok(new { Message = "Data Not Found", usersBranch, Data = false });
            }
        }
        
        [HttpGet("GetCompany")]
        public async Task<IActionResult> GetCompany()
        {
            var userDetails = await GetKey.GetSecurityKey(HttpContext);
            var usersCompany = await _users.GetCompany(userDetails);
            if (usersCompany != null)
            {
                return Ok(new { Message = "Data Found", usersCompany, Data = true });
            }
            else
            {
                return Ok(new { Message = "Data Not Found", usersCompany, Data = false });
            }
        }
        
        [HttpPost("GetUsersById")]
        public async Task<IActionResult> GetUsersById(TaskUsersDto taskUsersDto)
        {
            var userDetails = await GetKey.GetSecurityKey(HttpContext);
            var userDetailById = await _users.GetUsersById(taskUsersDto.UserId,userDetails);
            if(userDetailById != null)
            {
                return Ok(new { Message = "Data Found", userDetailById, Data = true });
            }
            else
            {
                return Ok(new { Message = "Data Not Found", userDetailById, Data = false });
            }
        }

        [HttpPost("CreateUsers")]
        public async Task<IActionResult> CreateUsers(TaskUsersDto taskUsersDto)
        {
            var userDetails = await GetKey.GetSecurityKey(HttpContext);
            var usersList = await _users.CreateUsers(taskUsersDto, userDetails);
            if (usersList != null)
            {
                return Ok(new { Message = "Data Found", usersList, Data = true });
            }
            else
            {
                return Ok(new { Message = "Data Not Found", usersList, Data = false });
            }
        }

        [HttpPut("UpdateUsers")]
        public async Task<IActionResult> UpdateUsers(TaskUsersDto taskUsersDto)
        {
            var userDetails = await GetKey.GetSecurityKey(HttpContext);
            var usersList = await _users.UpdateUsers(taskUsersDto, userDetails);
            if (usersList != null)
            {
                return Ok(new { Message = "Data Found", usersList, Data = true });
            }
            else
            {
                return Ok(new { Message = "Data Not Found", usersList, Data = false });
            }
        }

        [HttpGet("GetUsersGroupList")]
        public async Task<IActionResult> GetUsersGroupList()
        {
            var userDetails = await GetKey.GetSecurityKey(HttpContext);
            var usersGroupList = await _users.GetUsersGroupList(userDetails);
            if (usersGroupList != null)
            {
                return Ok(new { Message = "Data Found", usersGroupList, Data = true });
            }
            else
            {
                return Ok(new { Message = "Data Not Found", usersGroupList, Data = false });
            }
        }

        [HttpPost("CreateUsersGroup")]
        public async Task<IActionResult> CreateUsersGroup(TaskUsersDto taskUsersDto)
        {
            var userDetails = await GetKey.GetSecurityKey(HttpContext);
            var usersGroupList = await _users.CreateUsersGroup(taskUsersDto, userDetails);
            if (usersGroupList != null)
            {
                return Ok(new { Message = "Data Found", usersGroupList, Data = true });
            }
            else
            {
                return Ok(new { Message = "Data Not Found", usersGroupList, Data = false });
            }
        }
      
        [HttpPost("GetUsersGroupById")]
        public async Task<IActionResult> GetUsersGroupById(TaskUsersDto taskUsersDto)
        {
            var userDetails = await GetKey.GetSecurityKey(HttpContext);
            var usersGroupList = await _users.GetUsersGroupById(taskUsersDto, userDetails);
            if (usersGroupList != null)
            {
                return Ok(new { Message = "Data Found", usersGroupList, Data = true });
            }
            else
            {
                return Ok(new { Message = "Data Not Found", usersGroupList, Data = false });
            }
        }

        [HttpPost("UpdateUsersGroup")]
        public async Task<IActionResult> UpdateUsersGroup(TaskUsersDto taskUsersDto)
        {
            var userDetails = await GetKey.GetSecurityKey(HttpContext);
            var usersGroupList = await _users.UpdateUsersGroup(taskUsersDto, userDetails);
            if (usersGroupList != null)
            {
                return Ok(new { Message = "Data Found", usersGroupList, Data = true });
            }
            else
            {
                return Ok(new { Message = "Data Not Found", usersGroupList, Data = false });
            }
        }

        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(TaskUsersDto taskUsersDto)
        {
            var userDetails = await GetKey.GetSecurityKey(HttpContext);
            var userChangePassword = await _users.ChangePassword(taskUsersDto, userDetails);
            if (userChangePassword != null)
            {
                return Ok(new { Message = "Data Found", userChangePassword, Data = true });
            }
            else
            {
                return Ok(new { Message = "Data Not Found", userChangePassword, Data = false });
            }
        }

        [HttpGet("GetCompanyAccessPolicyWiseMenuData")]
        public async Task<ActionResult> GetCompanyAccessPolicyWiseMenuData()
        {
            var userDetails = await GetKey.GetSecurityKey(HttpContext);
            var accessPolicyWiseMenuData = await _users.GetCompanyAccessPolicyWiseMenuData(userDetails);
            if (accessPolicyWiseMenuData != null)
            {
                return Ok(new { Message = "Data Found", accessPolicyWiseMenuData, Data = true });
            }
            else
            {
                return Ok(new { Message = "Data Not Found", accessPolicyWiseMenuData, Data = false });
            }
        } 
        
        [HttpGet("GetCompanyUserAccessPolicyWiseMenuData")]
        public async Task<ActionResult> GetCompanyUserAccessPolicyWiseMenuData()
        {
            var userDetails = await GetKey.GetSecurityKey(HttpContext);
            var accessPolicyWiseMenuData = await _users.GetCompanyUserAccessPolicyWiseMenuData(userDetails);
            if (accessPolicyWiseMenuData != null)
            {
                return Ok(new { Message = "Data Found", accessPolicyWiseMenuData, Data = true });
            }
            else
            {
                return Ok(new { Message = "Data Not Found", accessPolicyWiseMenuData, Data = false });
            }
        }

        [HttpGet("GetUserGroupWiseAccessPolicyMenu")]
        public async Task<ActionResult> GetUserGroupWiseAccessPolicyMenu()
        {
            try
            {
                var userDetails = await GetKey.GetSecurityKey(HttpContext);
                var groupAccessMenu = await _users.GetUserGroupWiseAccessPolicyMenu(userDetails);
                if (groupAccessMenu != null)
                {
                    return Ok(new { Message = "Data Found", groupAccessMenu, Data = true });
                }
                else
                {
                    return Ok(new { Message = "Data Not Found", groupAccessMenu, Data = false });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _users.CommonMessage() });
            }
        }

        [HttpGet("GetUserWiseAccessPolicyMenu")]
        public async Task<ActionResult> GetUserWiseAccessPolicyMenu()
        {
            try
            {
                var userDetails = await GetKey.GetSecurityKey(HttpContext);
                var userAccessMenu = await _users.GetUserWiseAccessPolicy(userDetails);
                if (userAccessMenu != null)
                {
                    return Ok(new { Message = "Data Found", userAccessMenu, Data = true });
                }
                else
                {
                    return Ok(new { Message = "Data Not Found", userAccessMenu, Data = false });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _users.CommonMessage() });
            }
        }

        [HttpPost("CreateRoleWiseAccessPolicyMenu")]
        public async Task<ActionResult> CreateRoleWiseAccessPolicyMenu(AccessPolicyDto accessPolicyDto)
        {
            try
            {
                var userDetails = await GetKey.GetSecurityKey(HttpContext);
                var accessMenu = await _users.CreateRoleWiseAccessPolicyMenu(accessPolicyDto, userDetails);
                if (accessMenu != null)
                {
                    return Ok(new { Message = "Data Found", accessMenu, Data = true });
                }
                else
                {
                    return Ok(new { Message = "Data Not Found", accessMenu, Data = false });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _users.CommonMessage() });
            }
        }

        [HttpPut("UpdateSystemMenuById")]
        public async Task<ActionResult> UpdateSystemMenuById(AccessPolicyDto accessPolicyDto)
        {
            try
            {
                var userDetails = await GetKey.GetSecurityKey(HttpContext);
                var accessMenu = await _users.UpdateSystemMenuById(accessPolicyDto, userDetails);
                if (accessMenu != null)
                {
                    return Ok(new { Message = "Data Found", accessMenu, Data = true });
                }
                else
                {
                    return Ok(new { Message = "Data Not Found", accessMenu, Data = false });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _users.CommonMessage() });
            }
        }

        [HttpPost("GetMenuByGroup")]
        public async Task<ActionResult> GetMenuByGroup(AccessPolicyDto accessPolicyDto)
        {
            var userDetails = await GetKey.GetSecurityKey(HttpContext);
            var accessMenu = await _users.GetMenuByGroup(accessPolicyDto, userDetails);
            if (accessMenu != null)
            {
                return Ok(new { Message = "Data Found", accessMenu, Data = true });
            }
            else
            {
                return Ok(new { Message = "Data Not Found", accessMenu, Data = false });
            }
        }

        [HttpPost("CreateMenu")]
        public async Task<ActionResult> InsertSystemMenu(AccessPolicyDto accessPolicyDto)
        {
            var userDetails = await GetKey.GetSecurityKey(HttpContext);
            var accessMenu = await _users.InsertSystemMenu(accessPolicyDto, userDetails);
            if (accessMenu != null)
            {
                return Ok(new { Message = "Data Found", accessMenu, Data = true });
            }
            else
            {
                return Ok(new { Message = "Data Not Found", accessMenu, Data = false });
            }
        }

        [HttpPost("RoleWiseMenu")]
        public async Task<ActionResult> RoleWiseMenu(UserRoleDto userRole)
        {
            var userDetails = await GetKey.GetSecurityKey(HttpContext);
            var accessMenu = await _users.RoleWiseMenu(userRole, userDetails);
            if (accessMenu != null)
            {
                return Ok(new { Message = "Data Found", accessMenu, Data = true });
            }
            else
            {
                return Ok(new { Message = "Data Not Found", accessMenu, Data = false });
            }
        }

        [HttpPost("PostUserWiseMenu")]
        public async Task<ActionResult> PostUserWiseMenu(UserMenuDto userMenu)
        {
            var userDetails = await GetKey.GetSecurityKey(HttpContext);
            var accessMenu = await _users.PostUserWiseMenu(userMenu, userDetails);
            if (accessMenu != null)
            {
                return Ok(new { Message = "Data Found", accessMenu, Data = true });
            }
            else
            {
                return Ok(new { Message = "Data Not Found", accessMenu, Data = false });
            }
        }
        
        
    }
}
