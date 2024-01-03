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
    public class EmployeeListController : ControllerBase
    {
        private readonly IEmployeeList _employeeList;

        public EmployeeListController(IEmployeeList employeeList)
        {
            _employeeList = employeeList;
        }


        [HttpGet("GetEmployee")]
        public async Task<IActionResult> GetEmployees()
        {
            try
            {
                var userDetails = await GetKey.GetSecurityKey(HttpContext);
                var empList = await _employeeList.GetEmployees(userDetails);

                if (empList != null)
                {
                    return Ok(new { Message = "Data Found", empList, Data = true });
                }
                else
                {
                    return Ok(new { Message = "Data Not Found", empList, Data = false });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _employeeList.CommonMessage() });
            }
        }

        [HttpPost("GetEmployeeById")]
        public async Task<IActionResult> GetEmployeeById(TaskEmployeeId employee)
        {
            try
            {
                var userDetails = await GetKey.GetSecurityKey(HttpContext);
                var empList = await _employeeList.GetEmployeeById(employee.EmployeeId, userDetails);

                if (empList != null)
                {
                    return Ok(new { Message = "Data Found", empList, Data = true });
                }
                else
                {
                    return Ok(new { Message = "Data Not Found", empList, Data = false });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _employeeList.CommonMessage() });
            }
        }

        [HttpPost("GetEmployeeByProjectModule")]
        public async Task<IActionResult> GetEmployeeByProjectModule(TaskActivityProjectModule ProjectModule)
        {
            try
            {
                var userDetails = await GetKey.GetSecurityKey(HttpContext);
                var empList = await _employeeList.GetEmployeeByProjectModule(ProjectModule, userDetails);

                if (empList != null)
                {
                    return Ok(new { Message = "Data Found", empList, Data = true });
                }
                else
                {
                    return Ok(new { Message = "Data Not Found", empList, Data = false });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _employeeList.CommonMessage() });
            }
        }

        [HttpPost("CreateEmployee")]
        public async Task<IActionResult> CreateEmployee(PrEmployeeListDto employeeListDto)
        {
            try
            {
                var userDetails = await GetKey.GetSecurityKey(HttpContext);
                var empList = await _employeeList.CreateEmployee(employeeListDto, userDetails);
                if (empList != null)
                {
                    return Ok(new { Message = "Data Found", empList, Data = true });
                }
                else
                {
                    return Ok(new { Message = "Data Not Found", empList, Data = false });
                }
            }
            catch(Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _employeeList.CommonMessage() });
            }
        }
        
        [HttpPut("UpdateEmployee")]
        public async Task<IActionResult> UpdateEmployee(PrEmployeeListDto employeeListDto)
        {
            try
            {
                var userDetails = await GetKey.GetSecurityKey(HttpContext);
                var empList = await _employeeList.UpdateEmployee(employeeListDto, userDetails);
                if (empList != null)
                {
                    return Ok(new { Message = "Data Found", empList, Data = true });
                }
                else
                {
                    return Ok(new { Message = "Data Not Found", empList, Data = false });
                }
            }
            catch(Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(new { Message = await _employeeList.CommonMessage() });
            }
        }
    }
}