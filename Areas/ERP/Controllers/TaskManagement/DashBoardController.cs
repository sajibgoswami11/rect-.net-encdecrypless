using System;
using System.Threading.Tasks;
using BizWebAPI.Areas.ERP.Dtos.TaskManagement;
using BizWebAPI.Areas.ERP.Interfaces.TaskManagement;
using BizWebAPI.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;

namespace BizWebAPI.Areas.ERP.Controllers.TaskManagement
{
    [Authorize]
    [Area("ERP")]
    [ApiController]
    [Route("ERP/TaskManagement/[controller]")]
    public class DashBoardController : ControllerBase
    {
        private readonly IDashBoard _dashBoard;
        public DashBoardController(IDashBoard dashBoard)
        {
            _dashBoard = dashBoard;
        }

        // GET: api/DashBoard
        [HttpPost("GetSummary")]
        public async Task<IActionResult> GetSummary(DashBoardDto dashBoardDto)
        {
            try
            {
                var userDetails = await GetKey.GetSecurityKey(HttpContext);
                var dashBoard = await _dashBoard.GetSummary(dashBoardDto, userDetails);

                if (dashBoard != null)
                {
                    return Ok( new { Message = "Data Found", dashBoard, data = true });
                }
                else
                {
                    return Ok( new { Message = "Data Not Found", dashBoard, data = false });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _dashBoard.CommonMessage() });
            }
        }

        // GET: api/DashBoard
        [HttpGet("GetSummaryByLogin")]
        public async Task<IActionResult> GetSummaryByLogin()
        {
            try
            {
                var userDetails = await GetKey.GetSecurityKey(HttpContext);
                var dashBoard = await _dashBoard.GetSummaryByLogin(userDetails);

                if (dashBoard != null)
                {
                    return Ok(new { Message = "Data Found", dashBoard, data = true });
                }
                else
                {
                    return Ok(new { Message = "Data Not Found", dashBoard, data = false });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _dashBoard.CommonMessage() });
            }
        } 
        
        [HttpGet("DashboardOnProjectCountClickGetDetails")]
        public async Task<IActionResult> DashboardOnProjectCountClickGetDetails()
        {
            try
            {
                var userDetails = await GetKey.GetSecurityKey(HttpContext);
                var dashBoard = await _dashBoard.DashboardOnProjectCountClickGetDetails(userDetails);

                if (dashBoard != null)
                {
                    return Ok(new { Message = "Data Found", dashBoard, data = true });
                }
                else
                {
                    return Ok(new { Message = "Data Not Found", dashBoard, data = false });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _dashBoard.CommonMessage() });
            }
        }

        [HttpGet("GetProjectProgress")]
        public async Task<IActionResult> GetProjectProgress()
        {
            try
            {
                var userDetails = await GetKey.GetSecurityKey(HttpContext);
                var dashBoard = await _dashBoard.GetProjectProgress(userDetails);

                if (dashBoard != null)
                {
                    return Ok(new { Message = "Data Found", dashBoard, data = true });
                }
                else
                {
                    return Ok(new { Message = "Data Not Found", dashBoard, data = false });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _dashBoard.CommonMessage() });
            }
        }

        [HttpPost("GetDateRangeProjectProgressSummary")]
        public async Task<IActionResult> GetDateRangeProjectProgressSummary(DateRangeDto dateRange)
        {
            try
            {
                var userDetails = await GetKey.GetSecurityKey(HttpContext);
                var dashBoard = await _dashBoard.GetDateRangeProjectProgressSummary(dateRange, userDetails);
                if (dateRange != null)
                {
                    return Ok(new { Message = "Data Found", dashBoard, data = true });
                }
                else
                {
                    return Ok(new { Message = "Data Not Found", dashBoard, data = false });
                }
            }
            catch(Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _dashBoard.CommonMessage() });
            }
        }

        [HttpGet("DashboardOnModuleCountClickGetDetails")]
        public async Task<IActionResult> DashboardOnModuleCountClickGetDetails()
        {
            try
            {
                var userDetails = await GetKey.GetSecurityKey(HttpContext);
                var dashBoard = await _dashBoard.DashboardOnModuleCountClickGetDetails(userDetails);

                if (dashBoard != null)
                {
                    return Ok(new { Message = "Data Found", dashBoard, data = true });
                }
                else
                {
                    return Ok(new { Message = "Data Not Found", dashBoard, data = false });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _dashBoard.CommonMessage() });
            }
        }

        [HttpGet("DashboardOnTaskAssignCountClickGetDetails")]
        public async Task<IActionResult> DashboardOnTaskAssignCountClickGetDetails()
        {
            try
            {
                var userDetails = await GetKey.GetSecurityKey(HttpContext);
                var dashBoard = await _dashBoard.DashboardOnTaskAssignCountClickGetDetails(userDetails);

                if (dashBoard != null)
                {
                    return Ok(new { Message = "Data Found", dashBoard, data = true });
                }
                else
                {
                    return Ok(new { Message = "Data Not Found", dashBoard, data = false });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _dashBoard.CommonMessage() });
            }
        }

        [HttpGet("DashboardOnTaskCompleteCountClickGetDetails")]
        public async Task<IActionResult> DashboardOnTaskCompleteCountClickGetDetails()
        {
            try
            {
                var userDetails = await GetKey.GetSecurityKey(HttpContext);
                var dashBoard = await _dashBoard.DashboardOnTaskCompleteCountClickGetDetails(userDetails);

                if (dashBoard != null)
                {
                    return Ok(new { Message = "Data Found", dashBoard, data = true });
                }
                else
                {
                    return Ok(new { Message = "Data Not Found", dashBoard, data = false });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _dashBoard.CommonMessage() });
            }
        }
    }
}
