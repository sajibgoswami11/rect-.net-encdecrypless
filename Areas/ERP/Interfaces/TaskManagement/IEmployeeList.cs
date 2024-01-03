using BizWebAPI.Areas.ERP.Dtos;
using BizWebAPI.Areas.ERP.Dtos.TaskManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.Interfaces.TaskManagement
{
    public interface IEmployeeList
    {
        Task<string> CommonMessage();
        Task<IEnumerable<PrEmployeeListDto>> GetEmployees(RequestDetailDto userDetails);
        Task<IEnumerable<PrEmployeeListDto>> GetEmployeeByProjectModule(TaskActivityProjectModule ProjectModule, RequestDetailDto requestDetaildto);
        Task<string> CreateEmployee(PrEmployeeListDto employeeListDto, RequestDetailDto requestDetaildto);
        Task<string> UpdateEmployee(PrEmployeeListDto employeeListDto, RequestDetailDto requestDetaildto);
        Task<PrEmployeeListDto> GetEmployeeById(string EmployeeId, RequestDetailDto requestDetailDto);
    }
}
