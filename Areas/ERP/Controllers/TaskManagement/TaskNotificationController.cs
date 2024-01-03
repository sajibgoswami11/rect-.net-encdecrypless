using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BizWebAPI.Areas.ERP.Dtos.TaskManagement;
using BizWebAPI.Areas.ERP.Interfaces.TaskManagement;
using BizWebAPI.Areas.ERP.Models.TaskManagement;
using BizWebAPI.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BizWebAPI.Areas.ERP.Controllers.TaskManagement
{
    [Authorize]
    [Area("ERP")]
    [ApiController]
    [Route("ERP/TaskManagement/[controller]")]
    public class TaskNotificationController : ControllerBase
    {
        private readonly INotification _notification;
        public TaskNotificationController(INotification notification)
        {
            _notification = notification;
        }

        [HttpGet("GetNotification")]
        public async Task<ActionResult> GetNotification()
        {
            try
            {
                var userDetails = await GetKey.GetSecurityKey(HttpContext);
                var notification = await _notification.GetNotification(userDetails);

                if (notification != null)
                {
                    return Ok(new { Message = "Data Found", notification, data = true });
                }
                else
                {
                    return Ok(new { Message = "Data Not Found", notification, data = false });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _notification.CommonMessage() });
            }
        }

        [HttpPost("PostNotifications")]
        public async Task<ActionResult> PostNotifications(IEnumerable<TaskNotificationDto> taskNotificationDto)
        {
            try
            {
                var userDetails = await GetKey.GetSecurityKey(HttpContext);
                var notification = await _notification.PostNotifications(taskNotificationDto,userDetails);

                if (notification != null)
                {
                    return Ok(new { Message = "Data Found", notification, data = true });
                }
                else
                {
                    return Ok(new { Message = "Data Not Found", notification, data = false });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _notification.CommonMessage() });
            }
        }

        [HttpPost("TaskIdByAssignIdwise")]
        public async Task<ActionResult> TaskIdByAssignIdwise(TaskNotificationDto taskNotificationDto)
        {
            try
            {
                var userDetails = await GetKey.GetSecurityKey(HttpContext);
                var notification = await _notification.TaskIdByAssignIdwise(taskNotificationDto, userDetails);

                if (notification != null)
                {
                    return Ok(new { Message = "Data Found", notification, data = true });
                }
                else
                {
                    return Ok(new { Message = "Data Not Found", notification, data = false });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _notification.CommonMessage() });
            }
        }
    }
}
