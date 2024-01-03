using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BizWebAPI.Areas.ERP.Dtos.TaskManagement;
using BizWebAPI.Areas.ERP.Interfaces.TaskManagement;
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
    public class ReportsController : ControllerBase
    {
        private readonly IReports _reports;
        public ReportsController(IReports reports)
        {
            _reports = reports;
        }

        [HttpPost("TaskReport")]
        public async Task<IActionResult> TaskReport(TaskReportRequestDto taskReport)
        {
            try
            {
                var userDetails = await GetKey.GetSecurityKey(HttpContext);
                var report = await _reports.TaskReport(taskReport, userDetails);

                if (report != null)
                {
                    return Ok(new { Message = "Data Found", report, data = true });
                }
                else
                {
                    return Ok(new { Message = "Data Not Found", report, data = false });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _reports.CommonMessage() });
            }
        } 
        
        [HttpPost("ReportEmployeeActiveinactive")]
        public async Task<IActionResult> ReportEmployeeActiveinactive(TaskReportRequestDto taskReport)
        {
            try
            {
                var userDetails = await GetKey.GetSecurityKey(HttpContext);
                var report = await _reports.ReportEmployeeActiveinactive(taskReport, userDetails);

                if (report != null)
                {
                    return Ok(new { Message = "Data Found", report, data = true });
                }
                else
                {
                    return Ok(new { Message = "Data Not Found", report, data = false });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _reports.CommonMessage() });
            }
        }

       [HttpPost("PersonnelActivitySummary")]
       public async Task<IActionResult> PersonnelActivitySummary (TaskReportRequestDto reportRequestDto)
        {
            try
            {
                var userDetails = await GetKey.GetSecurityKey(HttpContext);
                var activitySummery = await _reports.PersonnelActivitySummary(reportRequestDto, userDetails);

                if (activitySummery != null)
                {
                    return Ok(new { Message = "Data Found", activitySummery, data = true });
                }
                else
                {
                    return Ok(new { Message = "Data Not Found", activitySummery, data = false });
                }
            }
            catch(Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _reports.CommonMessage() });

            }
        }

        [HttpPost("PersonnelActivitySummaryDetails")]
        public async Task<IActionResult> PersonnelActivitySummaryDetails(TaskReportRequestDto reportRequestDto)
        {
            try
            {
                var userDetails = await GetKey.GetSecurityKey(HttpContext);
                var activitySummeryDetails = await _reports.PersonnelActivitySummaryDetails(reportRequestDto, userDetails);

                if (activitySummeryDetails != null)
                {
                    return Ok(new { Message = "Data Found", activitySummeryDetails, data = true });
                }
                else
                {
                    return Ok(new { Message = "Data Not Found", activitySummeryDetails, data = false });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _reports.CommonMessage() });

            }
        }
    }
}
