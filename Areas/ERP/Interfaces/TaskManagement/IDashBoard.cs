using BizWebAPI.Areas.ERP.Dtos;
using BizWebAPI.Areas.ERP.Dtos.TaskManagement;
using BizWebAPI.Areas.ERP.Models.TaskManagement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.Interfaces.TaskManagement
{
    public interface IDashBoard
    {
        Task<string> CommonMessage();
        Task<IEnumerable<TaskListDto>> GetTaskList(RequestDetailDto requestDetaildto);
        Task<TaskSummaryInfoDto> GetSummary(DashBoardDto dashBoardDto, RequestDetailDto requestDetaildto);
        Task<IEnumerable<TaskForwardDto>> GetTaskForwardList(RequestDetailDto userDetails);
        Task<TaskSummaryInfoDto> GetSummaryByLogin(RequestDetailDto requestDetaildto);
        Task<IEnumerable<DashBoardPercentageDto>> GetProjectProgress(RequestDetailDto requestDetaildto);
        Task<TaskProjectProgressSummaryDto> GetDateRangeProjectProgressSummary(DateRangeDto dateRange, RequestDetailDto requestDetaildto);
        Task<IEnumerable<DashboardProjectDetailsDto>> DashboardOnProjectCountClickGetDetails(RequestDetailDto requestDetaildto);
        Task<IEnumerable<DashboardModuleDetailsDto>> DashboardOnModuleCountClickGetDetails(RequestDetailDto requestDetaildto);
        Task<IEnumerable<DashboardAssignDetailsDto>> DashboardOnTaskAssignCountClickGetDetails(RequestDetailDto requestDetaildto);
        Task<IEnumerable<DashboardCompleteDetailsDto>> DashboardOnTaskCompleteCountClickGetDetails(RequestDetailDto requestDetaildto);

    }
}
