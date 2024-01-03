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
    public class TaskForwardController : ControllerBase
    {
        private readonly ITaskForward _taskForwardService;

        public TaskForwardController(ITaskForward taskForwardService)
        {
            _taskForwardService = taskForwardService;
        }

        [HttpGet("GetTaskForwardList")]
        public async Task<IActionResult> GetTaskForwardList()
        {
            try
            {
                var userDetails = await GetKey.GetSecurityKey(HttpContext);
                var forwardTask = await _taskForwardService.GetTaskForwardList(userDetails);

                if (forwardTask != null)
                {
                    return Ok(new { Message = "Data Found", forwardTask, Data = true });
                }
                else
                {
                    return Ok(new { Message = "Data Not Found", forwardTask, Data = false });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _taskForwardService.CommonMessage() });
            }
        }
        
        [HttpPost("GetForwardTaskById")]
        public async Task<IActionResult> GetTaskForwardById(TaskForwardId Forward)
        {
            try
            {
                var userDetails = await GetKey.GetSecurityKey(HttpContext);
                var forwardTask = await _taskForwardService.GetTaskForwardById(Forward.ForwardId, userDetails);

                if (forwardTask != null)
                {
                    return Ok(new { Message = "Data Found", forwardTask, Data = true });
                }
                else
                {
                    return Ok(new { Message = "Data Not Found", forwardTask, Data = false });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _taskForwardService.CommonMessage() });
            }
        }

        [HttpPost("CreateTaskForward")]
        public async Task<IActionResult> CreateTaskForward(TaskForwardDto taskForwardDto)
        {
            try
            {
                var userDetails = await GetKey.GetSecurityKey(HttpContext);
                var forwardTask = await _taskForwardService.CreateTaskForward(taskForwardDto,  userDetails);

                if (forwardTask != null)
                {
                    return Ok(new { Message = "Data Found", forwardTask, Data = true });
                }
                else
                {
                    return Ok(new { Message = "Data Not Found", forwardTask, Data = false });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _taskForwardService.CommonMessage() });
            }
        }

        [HttpPut("UpdateTaskForwardByID")]
        public async Task<IActionResult> UpdateTaskForwardByID(TaskForwardDto taskForwardDto)
        {
            try
            {
                var userDetails = await GetKey.GetSecurityKey(HttpContext);
                var forwardTask = await _taskForwardService.UpdateTaskForwardByID(taskForwardDto, userDetails);

                if (forwardTask != null)
                {
                    return Ok(new { Message = "Data Found", forwardTask, Data = true });
                }
                else
                {
                    return Ok(new { Message = "Data Not Found", forwardTask, Data = false });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _taskForwardService.CommonMessage() });
            }
        }

        [HttpPut("DeleteTaskForwardById")]
        public async Task<IActionResult> DeleteTaskForwardById(TaskForwardId forward)
        {
            try
            {
                var userDetails = await GetKey.GetSecurityKey(HttpContext);
                var forwardTask = await _taskForwardService.DeleteTaskForwardById(forward.ForwardId, userDetails);

                if (forwardTask != null)
                {
                    return Ok(new { Message = "Data Found", forwardTask, Data = true });
                }
                else
                {
                    return Ok(new { Message = "Data Not Found", forwardTask, Data = false });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _taskForwardService.CommonMessage() });
            }
        }
    }
}
