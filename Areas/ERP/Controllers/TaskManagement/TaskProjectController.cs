using System;
using System.Linq;
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
    public class TaskProjectController : ControllerBase
    {
        private readonly ITaskProject _taskProject;

        public TaskProjectController(ITaskProject taskProject)
        {
            _taskProject = taskProject;
        }

        [HttpGet("GetTaskProject")]
        public async Task<IActionResult> GetTaskProject()
        {
            try
            {
                var userDetails = await GetKey.GetSecurityKey(HttpContext);
                var project = await _taskProject.GetTaskProject(userDetails);

                if (project != null)
                {
                    return Ok(new { Message = "Data Found", project, Data = true });
                }
                else
                {
                    return Ok(new { Message = "Data Not Found", project, Data = false });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _taskProject.CommonMessage() });
            }
        }

        // POST: /Project
        [HttpPost("CreateProject")]
        public async Task<IActionResult> CreateProject(TaskProjectDto projectDto)
        {
            try
            {
                var userDetails = await GetKey.GetSecurityKey(HttpContext);
                var project = await _taskProject.CreateProject(projectDto, userDetails);

                if (project != null)
                {
                    return Ok(new { Message = "Project create successful", project, Data = true });
                }
                else
                {
                    return Ok(new { Message = "project create failed", project, Data = false });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _taskProject.CommonMessage() });
            }
        }

        [HttpPut("UpdateProjectByID")]
        public async Task<IActionResult> UpdateProjectByID(TaskProjectDto taskProjectDto)
        {
            try
            {
                var userDetails = await GetKey.GetSecurityKey(HttpContext);
                var project = await _taskProject.UpdateProjectByID(taskProjectDto, userDetails);

                if (project != null)
                {
                    return Ok(new { Message = "Data Found", project, Data = true });
                }
                else
                {
                    return Ok(new { Message = "Data Not Found", project, Data = false });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _taskProject.CommonMessage() });
            }
        }

        [HttpPost("GetProjectById")]
        public async Task<IActionResult> GetProjectById(TaskProjectId Project)
        {
            try
            {
                var userDetails = await GetKey.GetSecurityKey(HttpContext);
                var project = await _taskProject.GetProjectById(Project.ProjectId, userDetails);

                if (project != null)
                {
                    return Ok(new { Message = "Data Found", project, Data = true });
                }
                else
                {
                    return Ok(new { Message = "Data Not Found", project, Data = false });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _taskProject.CommonMessage() });
            }
        }

        [HttpPost("GetProjectWiseTeamMember")]
        public async Task<IActionResult> GetProjectWiseTeamMember(TaskProjectId Project)
        {
            try
            {
                var userDetails = await GetKey.GetSecurityKey(HttpContext);
                var project = await _taskProject.GetProjectWiseTeamMember(Project.ProjectId, userDetails);

                if (project != null)
                {
                    return Ok(new { Message = "Data Found", project, Data = true });
                }
                else
                {
                    return Ok(new { Message = "Data Not Found", project, Data = false });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _taskProject.CommonMessage() });
            }
        }

        [HttpPut("DeleteProjectID")]
        public async Task<IActionResult> DeleteProjectById(TaskProjectId Project)
        {
            try
            {
                var userDetails = await GetKey.GetSecurityKey(HttpContext);
                var project = await _taskProject.DeleteProjectById(Project.ProjectId, userDetails);

                if (project != null)
                {
                    return Ok(new { Message = "Data Found", project, Data = true });
                }
                else
                {
                    return Ok(new { Message = "Data Not Found", project, Data = false });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _taskProject.CommonMessage() });
            }
        }
    }
}
