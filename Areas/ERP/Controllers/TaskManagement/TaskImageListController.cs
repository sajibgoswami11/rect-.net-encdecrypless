using System;
using System.Collections.Generic;
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
    public class TaskImageListController : Controller
    {
        private readonly ITaskImageList _taskImageList;
        public TaskImageListController(ITaskImageList taskListService)
        {
            _taskImageList = taskListService;
        }

        [HttpPost("GetTaskImageById")]
        public async Task<IActionResult> GetTaskImageById(TaskListId taskList)
        {
            try
            {
                var userDetails = await GetKey.GetSecurityKey(HttpContext);
                var image = await _taskImageList.GetTaskImageById(taskList.TaskId, userDetails);

                if (image != null)
                {
                    return Ok(new { Message = "Data Found", image, Data = true });
                }
                else
                {
                    return Ok(new { Message = "Data Not Found", image, Data = false });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _taskImageList.CommonMessage() });
            }
        }

        [HttpPost("DeleteTaskImageById")]
        public async Task<IActionResult> DeleteTaskImageById(TaskListId taskList)
        {
            try
            {
                var userDetails = await GetKey.GetSecurityKey(HttpContext);
                var image = await _taskImageList.DeleteTaskImageById(taskList.ImageId, userDetails);

                if (image != null)
                {
                    return Ok(new { Message = "Data Found", image, Data = true });
                }
                else
                {
                    return Ok(new { Message = "Data Not Found", image, Data = false });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _taskImageList.CommonMessage() });
            }
        }
    }
}
