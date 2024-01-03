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
    public class TeamController : ControllerBase
    {
        private readonly ITeamEmpMapping _team;
        public TeamController(ITeamEmpMapping team)
        {
            _team = team;
        }

        #region task team
        // GET: api/Team
       
       
        // GET: api/Team/5
        [HttpGet("GetTaskTeams")]
        public async Task<IActionResult> GetTaskTeams()
        {
            try
            {
                var userDetails = await GetKey.GetSecurityKey(HttpContext);
                var taskTeam = await _team.GetTaskTeams(userDetails);

                if (taskTeam != null)
                {
                    return Ok(new { Message = "Data Found", taskTeam, Data = true });
                }
                else
                {
                    return Ok(new { Message = "Data Not Found", taskTeam, Data = false });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _team.CommonMessage() });
            }
        } 
        
        [HttpPost("GetTaskTeamById")]
        public async Task<IActionResult> GetTaskTeamById(TaskTeamId Team)
        {
            try
            {
                var userDetails = await GetKey.GetSecurityKey(HttpContext);
                var taskTeam = await _team.GetTaskTeamById(Team.TeamId, userDetails);

                if (taskTeam != null)
                {
                    return Ok(new { Message = "Data Found", taskTeam, Data = true });
                }
                else
                {
                    return Ok(new { Message = "Data Not Found", taskTeam, Data = false });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _team.CommonMessage() });
            }
        }
    
        // POST: api/Team
        [HttpPost("CreateTeam")]
        public async Task<IActionResult> CreateTeam(TeamCreateDto taskTeamDto)
        {
            try
            {
                var userDetails = await GetKey.GetSecurityKey(HttpContext);
                var taskTeam = await _team.CreateTeam(taskTeamDto, userDetails);

                if (taskTeam != null)
                {
                    return Ok(new { Message = "Data Found", taskTeam, Data = true });
                }
                else
                {
                    return Ok(new { Message = "Data Not Found", taskTeam, Data = false });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _team.CommonMessage() });
            }
        }

        [HttpPut("UpdateTeam")] 
        public async Task<IActionResult> UpdateTeam(TeamCreateDto taskTeamDto)
        {
            try
            {
                var userDetails = await GetKey.GetSecurityKey(HttpContext);
                var taskTeam = await _team.UpdateTeam(taskTeamDto, userDetails);

                if (taskTeam != null)
                {
                    return Ok(new { Message = "Data Found", taskTeam, Data = true });
                }
                else
                {
                    return Ok(new { Message = "Data Not Found", taskTeam, Data = false });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _team.CommonMessage() });
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpPut("DeleteTaskTeams")]
        public async Task<IActionResult> DeleteTaskTeams(TaskTeamId Team)
        {
            try
            {
                var userDetails = await GetKey.GetSecurityKey(HttpContext);
                var taskTeam = await _team.DeleteTaskTeams(Team.TeamId, userDetails);

                if (taskTeam != null)
                {
                    return Ok(new { Message = "Record has been deleted.", taskTeam, Data = true });
                }
                else
                {
                    return Ok(new { Message = "Data Not Found", taskTeam, Data = false });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _team.CommonMessage() });
            }
        }

        #endregion

        #region teamwiseemployee

        [HttpPost("GetTeamWiseEmployeeMapping")]
        public async Task<IActionResult> GetTeamWiseEmployeeMapping(TaskTeamId Team)
        {
            try
            {
                var userDetails = await GetKey.GetSecurityKey(HttpContext);
                var taskTeam = await _team.GetTeamWiseEmployeeMapping(Team.TeamId, userDetails);

                if (taskTeam != null)
                {
                    return Ok(new { Message = "Data Found", taskTeam, Data = true });
                }
                else
                {
                    return Ok(new { Message = "Data Not Found", taskTeam, Data = false });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _team.CommonMessage() });
            }
        }

        #endregion
    }
}
