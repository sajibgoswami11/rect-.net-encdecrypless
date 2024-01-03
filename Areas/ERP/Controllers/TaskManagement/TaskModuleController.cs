using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using BizWebAPI.Areas.ERP.Dtos.TaskManagement;
using BizWebAPI.Areas.ERP.Interfaces.TaskManagement;
using BizWebAPI.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BizWebAPI.Areas.ERP.Controllers.TaskManagement
{
    [Authorize]
    [Area("ERP")]
    [ApiController]
    [Route("ERP/TaskManagement/[controller]")]
    public class TaskModuleController : ControllerBase
    {
        private readonly ITaskModule _taskModule;

        public TaskModuleController(ITaskModule taskModule)
        {
            _taskModule = taskModule;
        }

        // GET: api/TaskModule
        [HttpGet("GetTaskModule")]
        public async Task<IActionResult> GetTaskModule()
        {
            try
            {
                var userDetails = await GetKey.GetSecurityKey(HttpContext);
                var module = await _taskModule.GetTaskModule(userDetails);

                if (module != null)
                {
                    return Ok(new { Message = "Data Found", module, Data = true });
                }
                else
                {
                    return Ok(new { Message = "Data Not Found", module, Data = false });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _taskModule.CommonMessage() });
            }
        }

        // GET: api/TaskModule/5
        [HttpPost("GetModuleById")]
        public async Task<IActionResult> GetModuleById(TaskModuleId moduleById)
        {
            try
            {
                var userDetails = await GetKey.GetSecurityKey(HttpContext);
                var module = await _taskModule.GetModuleById(moduleById.ModuleId, userDetails);

                if (module != null)
                {
                    return Ok(new { Message = "Data Found", module, Data = true });
                }
                else
                {
                    return Ok(new { Message = "Data Not Found", module, Data = false });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _taskModule.CommonMessage() });
            }
        } 
        
        [HttpPost("GetPersonsOfModuleByAssignId")]
        public async Task<IActionResult> GetPersonsOfModuleByAssignId(TaskAssignId taskAssign)
        {
            try
            {
                var userDetails = await GetKey.GetSecurityKey(HttpContext);
                var modulePersons = await _taskModule.GetPersonsOfModuleByAssignId(taskAssign.AssignId ,userDetails);

                if (modulePersons != null)
                {
                    return Ok(new { Message = "Data Found", modulePersons, Data = true });
                }
                else
                {
                    return Ok(new { Message = "Data Not Found", modulePersons, Data = false });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _taskModule.CommonMessage() });
            }
        }

        [HttpPost("GetModuleListByProjectId")]
        public async Task<IActionResult> GetModuleListByProjectId(TaskProjectId projectById)
        {
            try
            {
                var userDetails = await GetKey.GetSecurityKey(HttpContext);
                var module = await _taskModule.GetModuleListByProjectId(projectById.ProjectId, userDetails);

                if (module != null)
                {
                    return Ok(new { Message = "Data Found", module, Data = true });
                }
                else
                {
                    return Ok(new { Message = "Data Not Found", module, Data = false });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _taskModule.CommonMessage() });
            }
        }

        [HttpPost("GetModuleWiseTeamMember")]
        public async Task<IActionResult> GetModuleWiseTeamMember(TaskModuleId moduleById)
        {
            try
            {
                var userDetails = await GetKey.GetSecurityKey(HttpContext);
                var teamMember = await _taskModule.GetModuleWiseTeamMember(moduleById.ModuleId, userDetails);

                if (teamMember != null)
                {
                    return Ok(new { Message = "Data Found", teamMember, Data = true });
                }
                else
                {
                    return Ok(new { Message = "Data Not Found", teamMember, Data = false });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _taskModule.CommonMessage() });
            }
        }

        // POST: api/TaskModule
        [HttpPost("CreateModule")]
        public async Task<IActionResult> CreateModule(IEnumerable<TaskModuleDto> taskModuleDto)
        {
            try
            {
                var userDetails = await GetKey.GetSecurityKey(HttpContext);
                var module = await _taskModule.CreateModule(taskModuleDto, userDetails);

                if (module != null)
                {
                    return Ok(new { Message = "Data Found", module, Data = true });
                }
                else
                {
                    return Ok(new { Message = "Data Not Found", module, Data = false });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _taskModule.CommonMessage() });
            }
        }

        // PUT: api/TaskModule/5
        [HttpPost("UpdateModuleByID")]
        public async Task<IActionResult> UpdateModuleByID(TaskModuleDto taskModuleDto)
        {
            try
            {
                var userDetails = await GetKey.GetSecurityKey(HttpContext);
                var module = await _taskModule.UpdateModuleByID(taskModuleDto, userDetails);

                if (module != null)
                {
                    return Ok(new { Message = "Data Found", module, Data = true });
                }
                else
                {
                    return Ok(new { Message = "Data Not Found", module, Data = false });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _taskModule.CommonMessage() });
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpPut("DeleteModuleByID")]
        public async Task<IActionResult> DeleteModuleByID(TaskModuleId moduleById)
        {
            try
            {
                var userDetails = await GetKey.GetSecurityKey(HttpContext);
                var module = await _taskModule.DeleteModuleById(moduleById.ModuleId, userDetails);

                if (module != null)
                {
                    return Ok(new { Message = "Data Found", module, Data = true });
                }
                else
                {
                    return Ok(new { Message = "Data Not Found", module, Data = false });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _taskModule.CommonMessage() });
            }
        }
    }
}
