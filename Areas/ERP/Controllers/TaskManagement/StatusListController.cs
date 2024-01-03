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
    public class StatusListController : ControllerBase
    {
        private readonly IStatusList _statusList;
        public StatusListController(IStatusList statusList)
        {
            _statusList = statusList;
        }

        #region task_status_list

        // GET: api/StatusList
        [HttpGet("GetStatusList")]
        public async Task<IActionResult> GetStatusList()
        {
            try
            {
                var userDetails = await GetKey.GetSecurityKey(HttpContext);
                var statusDetails = await _statusList.GetStatusList(userDetails);

                if (statusDetails != null)
                {
                    return Ok(new { Message = "Data Found", statusDetails, data = true });
                }
                else
                {
                    return Ok( new { Message = "Data Not Found", statusDetails, Data = false });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _statusList.CommonMessage() });
            }
        }

        // GET: api/StatusList/5
        [HttpPost("GetStatusByID")]
        public async Task<IActionResult> GetStatusByID(TaskStatusListId status)
        {
            try
            {
                var userDetails = await GetKey.GetSecurityKey(HttpContext);
                var statusDetails = await _statusList.GetStatusByID(status.StatusId, userDetails);

                if (statusDetails != null)
                {
                    return Ok(new { Message = "Data Found", statusDetails, Data = true });
                }
                else
                {
                    return Ok(new { Message = "Data Not Found", statusDetails, Data = false });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _statusList.CommonMessage() });
            }
        }

        [HttpPost("GetCategoryWiseStatusList")]
        public async Task<IActionResult> GetCategoryWiseStatusList(TaskStatusCategoryId category)
        {
            try
            {
                var userDetails = await GetKey.GetSecurityKey(HttpContext);
                var statusDetails = await _statusList.GetCategoryWiseStatusList(category.CategoryId, userDetails);

                if (statusDetails != null)
                {
                    return Ok(new { Message = "Data Found", statusDetails, Data = true });
                }
                else
                {
                    return Ok(new { Message = "Data Not Found", statusDetails, Data = false });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _statusList.CommonMessage() });
            }
        }

        // POST: api/StatusList
        [HttpPost("CreateStatus")]
        public async Task<IActionResult> CreateStatus(TaskStatusListDto taskStatusListDto)
        {
            try
            {
                var userDetails = await GetKey.GetSecurityKey(HttpContext);
                var statusDetails = await _statusList.CreateStatus(taskStatusListDto, userDetails);

                if (statusDetails != null)
                {
                    return Ok(new { Message = "Data Found", statusDetails, Data = true });
                }
                else
                {
                    return Ok(new { Message = "Data Not Found", statusDetails, Data = false });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _statusList.CommonMessage() });
            }
        }

        // PUT: api/StatusList/5
        [HttpPut("UpdteStatusById")]
        public async Task<IActionResult> UpdteStatusById(TaskStatusListDto taskStatusListDto)
        {
            try
            {
                var userDetails = await GetKey.GetSecurityKey(HttpContext);
                var statusDetails = await _statusList.UpdteStatusById(taskStatusListDto, userDetails);

                if (statusDetails != null)
                {
                    return Ok(new { Message = "Data Found", statusDetails, Data = true });
                }
                else
                {
                    return Ok(new { Message = "Data Not Found", statusDetails, Data = false });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _statusList.CommonMessage() });
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpPut("DeleteStatus")]
        public async Task<IActionResult> DeleteStatus(TaskStatusListId status)
        {
            try
            {
                var userDetails = await GetKey.GetSecurityKey(HttpContext);
                var statusDetails = await _statusList.DeleteStatusById(status.StatusId, userDetails);

                if (statusDetails != null)
                {
                    return Ok(new { Message = "Data Found", statusDetails, Data = true });
                }
                else
                {
                    return Ok(new { Message = "Data Not Found", statusDetails, Data = false });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _statusList.CommonMessage() });
            }
        }

        #endregion
    }
}
