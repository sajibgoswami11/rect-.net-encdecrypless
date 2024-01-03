using System;
using System.Linq;
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
    public class TaskActivityController : ControllerBase
    {
        private readonly ITaskActivity _taskActivityService;

        public TaskActivityController(ITaskActivity taskActivity)
        {
            _taskActivityService = taskActivity;
        }

        [HttpGet("GetTaskActivityList")]
        public async Task<IActionResult> GetTaskActivityList()
        {
            try
            {
                var userDetails = await GetKey.GetSecurityKey(HttpContext);
                var taskActivities = await _taskActivityService.GetTaskActivityList(userDetails);

                if (taskActivities.Count() > 0)
                {
                    return Ok( new { Message = "Data Found", taskActivities, Data = true });
                }
                else
                {
                    return Ok( new { Message = "Data Not Found", taskActivities, Data = false });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _taskActivityService.CommonMessage() });
            }
        }

        [HttpPost("CreateTaskActivity")]
        public async Task<IActionResult> CreateTaskActivity(TaskActivityRequestDto taskActivityDto)
        {
            try
            {
                var userDetails = await GetKey.GetSecurityKey(HttpContext);
                var taskActivities = await _taskActivityService.CreateTaskActivity(taskActivityDto, userDetails);

                if (taskActivities != null)
                {
                    return Ok(new { Message = "Data Found", taskActivities, Data = true });
                }
                else
                {
                    return Ok(new { Message = "Data Not Found", taskActivities, Data = false });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _taskActivityService.CommonMessage() });
            }
        }

        [HttpPut("UpdateTaskActivityByID")]
        public async Task<IActionResult> UpdateTaskActivityByID(TaskActivityDto taskActivityDto)
        {
            try
            {
                var userDetails = await GetKey.GetSecurityKey(HttpContext);
                var taskActivities = await _taskActivityService.UpdateTaskActivityByID(taskActivityDto, userDetails);

                if (taskActivities != null)
                {
                    return Ok(new { Message = "Data Found", taskActivities, Data = true });
                }
                else
                {
                    return Ok(new { Message = "Data Not Found", taskActivities, Data = false });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _taskActivityService.CommonMessage() });
            }
        }

        [HttpPost("GetTaskActivityById")]
        public async Task<IActionResult> GetTaskActivityById(TaskActivityId activity)
        {
            try
            {
                var userDetails = await GetKey.GetSecurityKey(HttpContext);
                var taskActivities = await _taskActivityService.GetTaskActivityById(activity.ActivityId, userDetails);

                if (taskActivities != null)
                {
                    return Ok(new { Message = "Data Found", taskActivities, Data = true });
                }
                else
                {
                    return Ok(new { Message = "Data Not Found", taskActivities, Data = false });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _taskActivityService.CommonMessage() });
            }
        }

        [HttpPost("TaskActivityByProjectModule")]
        public async Task<IActionResult> TaskActivityByProjectModule(TaskActivityProjectModule projectModule)
        {
            try
            {
                var userDetails = await GetKey.GetSecurityKey(HttpContext);
                var taskActivities = await _taskActivityService.TaskActivityByProjectModule(projectModule, userDetails);

                if (taskActivities.Count() > 0)
                {
                    return Ok(new { Message = "Data Found", taskActivities, Data = true });
                }
                else
                {
                    return Ok(new { Message = "Data Not Found", taskActivities, Data = false });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _taskActivityService.CommonMessage() });
            }
        }

        [HttpPut("DeleteTaskActivityById")]
        public async Task<IActionResult> DeleteTaskActivityById(TaskActivityId activity)
        {
            try
            {
                var userDetails = await GetKey.GetSecurityKey(HttpContext);
                var taskActivities = await _taskActivityService.DeleteTaskActivityById(activity.ActivityId, userDetails);

                if (taskActivities != null)
                {
                    return Ok(new { Message = "Data Found", taskActivities, Data = true });
                }
                else
                {
                    return Ok(new { Message = "Data Not Found", taskActivities, Data = false });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _taskActivityService.CommonMessage() });
            }
        }

        [HttpGet("GetTaskCompletebyAdmin")]
        public async Task<IActionResult> GetTaskCompletebyAdmin()
        {
            try 
            {
                var userDetails = await GetKey.GetSecurityKey(HttpContext);
                var taskCompleteByAdminDetails = await _taskActivityService.GetTaskCompletebyAdmin(userDetails);

                if (taskCompleteByAdminDetails.Count() > 0)
                {
                    return Ok(new { Message = "Data Found", taskCompleteByAdminDetails, Data = true });
                }
                else
                {
                    return Ok(new { Message = "Data Not Found", taskCompleteByAdminDetails, Data = false });
                }
            }
            catch(Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _taskActivityService.CommonMessage() });


            }
        }
    }
}
