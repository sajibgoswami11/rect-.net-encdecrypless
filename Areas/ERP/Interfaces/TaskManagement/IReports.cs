using BizWebAPI.Areas.ERP.Dtos;
using BizWebAPI.Areas.ERP.Dtos.TaskManagement;
using BizWebAPI.Areas.ERP.Models.TaskManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.Interfaces.TaskManagement
{
    public interface IReports
    {
        Task<string> CommonMessage();
        Task<IEnumerable<TaskReportDto>> TaskReport(TaskReportRequestDto taskReport, RequestDetailDto requestDetaildto);
        Task<IEnumerable<TaskReportDto>> ReportEmployeeActiveinactive(TaskReportRequestDto taskReport, RequestDetailDto requestDetaildto);
        Task<IEnumerable<TaskReportDto>> PersonnelActivitySummary(TaskReportRequestDto reportRequestDto, RequestDetailDto requestDetaildto);
        Task<IEnumerable<TaskReportDto>> PersonnelActivitySummaryDetails(TaskReportRequestDto reportRequestDto, RequestDetailDto requestDetaildto);
    }
}
