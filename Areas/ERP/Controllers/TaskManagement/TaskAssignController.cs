using System;
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
    public class TaskAssignController : ControllerBase
    {
        private readonly ITaskAssign _taskAssignService;

        public TaskAssignController(ITaskAssign taskAssignService)
        {
            _taskAssignService = taskAssignService;
        }

        [HttpGet("GetTaskAssignList")]
        public async Task<IActionResult> GetTaskAssignList()
        {
            try
            {
                var userDetails = await GetKey.GetSecurityKey(HttpContext);
                var assignTasks = await _taskAssignService.GetTaskAssignList(userDetails);

                if (assignTasks != null)
                {
                    return Ok( new { Message = "Data Found", assignTasks, Data = true });
                }
                else
                {
                    return Ok( new { Message = "Data Not Found", assignTasks, Data = false });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _taskAssignService.CommonMessage() });
            }
        }

        [HttpGet("GetTaskAssignWiseName")]
        public async Task<IActionResult> GetTaskAssignWiseName()
        {
            try
            {
                var userDetails = await GetKey.GetSecurityKey(HttpContext);
                var assignTasks = await _taskAssignService.GetTaskAssignWiseName(userDetails);

                if (assignTasks != null)
                {
                    return Ok(new { Message = "Data Found", assignTasks, Data = true });
                }
                else
                {
                    return Ok(new { Message = "Data Not Found", assignTasks, Data = false });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _taskAssignService.CommonMessage() });
            }
        }

        [HttpPost("CreateTaskAssign")]
        public async Task<IActionResult> CreateTaskAssign(TaskAssignDto taskTaskAssignDto)
        {
            try
            {
                var userDetails = await GetKey.GetSecurityKey(HttpContext);
                var assignTasks = await _taskAssignService.CreateTaskAssign(taskTaskAssignDto, userDetails);

                if (assignTasks != null)
                {
                    return Ok(new { Message = "Data Found", assignTasks, Data = true });
                }
                else
                {
                    return Ok(new { Message = "Data Not Found", assignTasks, Data = false });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _taskAssignService.CommonMessage() });
            }
        }

        [HttpPut("UpdateTaskAssignByID")]
        public async Task<IActionResult> UpdateTaskAssignByID(TaskAssignDto taskTaskAssignDto)
        {
            try
            {
                var userDetails = await GetKey.GetSecurityKey(HttpContext);
                var assignTasks = await _taskAssignService.UpdateTaskAssignByID(taskTaskAssignDto, userDetails);

                if (assignTasks != null)
                {
                    return Ok(new { Message = "Data Found", assignTasks, Data = true });
                }
                else
                {
                    return Ok(new { Message = "Data Not Found", assignTasks, Data = false });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _taskAssignService.CommonMessage() });
            }
        }

        [HttpPost("GetAssignTaskById")]
        public async Task<IActionResult> GetAssignTaskById(TaskAssignId assign)
        {
            try
            {
                var userDetails = await GetKey.GetSecurityKey(HttpContext);
                var assignTasks = await _taskAssignService.GetTaskAssignById(assign.AssignId, userDetails);

                if (assignTasks != null)
                {
                    return Ok(new { Message = "Data Found", assignTasks, Data = true });
                }
                else
                {
                    return Ok(new { Message = "Data Not Found", assignTasks, Data = false });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _taskAssignService.CommonMessage() });
            }
        }

        [HttpPost("TaskSatarPause")]
        public async Task<IActionResult> TaskSatarPause(TaskTempActivitryDto tempActivity)
        {
            try
            {
                var userDetails = await GetKey.GetSecurityKey(HttpContext);
                var taskSatarPause = await _taskAssignService.TaskSatarPause(tempActivity, userDetails);

                if (taskSatarPause != null)
                {
                    return Ok(new { Message = "Data Found", taskSatarPause, Data = true });
                }
                else
                {
                    return Ok(new { Message = "Data Not Found", taskSatarPause, Data = false });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _taskAssignService.CommonMessage() });
            }
        }

        [HttpPost("AssignTimeExtend")]
        public async Task<IActionResult> AssignTimeExtend(AssignTimeExtendDto assignTimeExtend)
        {
            try
            {
                var userDetails = await GetKey.GetSecurityKey(HttpContext);
                var TimeExtend = await _taskAssignService.AssignTimeExtend(assignTimeExtend, userDetails);

                if (TimeExtend != null)
                {
                    return Ok(new { Message = "Data Found", TimeExtend, Data = true });
                }
                else
                {
                    return Ok(new { Message = "Data Not Found", TimeExtend, Data = false });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _taskAssignService.CommonMessage() });
            }
        }

        [HttpPut("DeleteTaskAssignById")]
        public async Task<IActionResult> DeleteTaskAssignById(TaskAssignId assign)
        {
            try
            {
                var userDetails = await GetKey.GetSecurityKey(HttpContext);
                var assignTasks = await _taskAssignService.DeleteTaskAssignById(assign.AssignId, userDetails);

                if (assignTasks != null)
                {
                    return Ok(new { Message = "Data Found", assignTasks, Data = true });
                }
                else
                {
                    return Ok(new { Message = "Data Not Found", assignTasks, Data = false });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _taskAssignService.CommonMessage() });
            }
        }

        
    }
}
