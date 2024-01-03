using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BizWebAPI.Areas.ERP.Dtos.TaskManagement;
using BizWebAPI.Areas.ERP.Interfaces.TaskManagement;
using BizWebAPI.Areas.ERP.Models.TaskManagement;
using BizWebAPI.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BizWebAPI.Areas.ERP.Controllers.TaskManagement
{
    [Authorize]
    [Area("ERP")]
    [ApiController]
    [Route("ERP/TaskManagement/[controller]")]
    public class TaskListController : ControllerBase
    {
        private readonly ITaskList _taskListService;
        public TaskListController(ITaskList taskListService)
        {
            _taskListService = taskListService;
        }

        [HttpGet("GetTaskList")]
        public async Task<IActionResult> GetTaskList()
        {
            try
            {
                var userDetails = await GetKey.GetSecurityKey(HttpContext);
                var taskList = await _taskListService.GetTaskList(userDetails);

                if (taskList != null)
                {
                    return Ok(new { Message = "Data Found", taskList, Data = true });
                }
                else
                {
                    return Ok(new { Message = "Data Not Found", taskList, Data = false });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _taskListService.CommonMessage() });
            }
        }

        // POST: /Task
        [HttpPost("CreateTask")]
        public async Task<IActionResult> CreateTask(IEnumerable<TaskCreateAssignDto> taskListDto)
        {
            try
            {
                var userDetails = await GetKey.GetSecurityKey(HttpContext);
                var taskList = await _taskListService.CreateTask(taskListDto, userDetails);

                if (taskList != null)
                {
                    return Ok(new { Message = "Data Found", taskList, Data = true });
                }
                else
                {
                    return Ok(new { Message = "Data Not Found", taskList, Data = false });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _taskListService.CommonMessage() });
            }
        }

        [HttpPost("GetTaskById")]
        public async Task<IActionResult> GetTaskById(TaskListId taskList)
        {
            try
            {
                var userDetails = await GetKey.GetSecurityKey(HttpContext);
                var task = await _taskListService.GetTaskById(taskList.TaskId, userDetails);

                if (task != null)
                {
                    return Ok(new { Message = "Data Found", task, Data = true });
                }
                else
                {
                    return Ok(new { Message = "Data Not Found", task, Data = false });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _taskListService.CommonMessage() });
            }
        }

        [HttpPut("UpdateTaskByID")]
        public async Task<IActionResult> UpdateTaskByID(TaskCreateAssignDto taskListDto)
        {
            try
            {
                var userDetails = await GetKey.GetSecurityKey(HttpContext);
                var taskList = await _taskListService.UpdateTaskByID(taskListDto, userDetails);

                if (taskList != null)
                {
                    return Ok(new { Message = "Data Found", taskList, Data = true });
                }
                else
                {
                    return Ok(new { Message = "Data Not Found", taskList, Data = false });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _taskListService.CommonMessage() });
            }
        }

        [HttpPost("DeleteTaskById")]
        public async Task<IActionResult> DeleteTaskById(TaskListId taskListById)
        {
            try
            {
                var userDetails = await GetKey.GetSecurityKey(HttpContext);
                var taskList = await _taskListService.DeleteTaskById(taskListById.TaskId, userDetails);

                if (taskList != null)
                {
                    return Ok(new { Message = "Data Found", taskList, Data = true });
                }
                else
                {
                    return Ok(new { Message = "Data Not Found", taskList, Data = false });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _taskListService.CommonMessage() });
            }
        }

    }   
}