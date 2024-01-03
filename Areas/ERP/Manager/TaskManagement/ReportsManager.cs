using AutoMapper;
using BizWebAPI.Areas.ERP.Dtos;
using BizWebAPI.Areas.ERP.Dtos.TaskManagement;
using BizWebAPI.Areas.ERP.Interfaces.TaskManagement;
using BizWebAPI.Areas.ERP.Models.TaskManagement;
using BizWebAPI.Areas.ERP.Repository;
using BizWebAPI.Areas.ERP.Repository.TaskManagement;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.Manager.TaskManagement
{
    public class ReportsManager : IReports
    {
        //private TaskNotificationRepoitory _taskNotificationRepository;

        private CommonRepository _commonRepository;
        private EmployeeListRepository _getEmployeeRepository;
        private DashBoardRepository _dashBoardRepository;
        private ReportsRepository _reportsRepository;
        private readonly IMapper _mapper;

        public ReportsManager(IMapper mapper)
        {
            _mapper = mapper;
        }
        public async Task<string> CommonMessage()
        {
            _commonRepository = new CommonRepository();
            return await _commonRepository.CommonMessage();
        }

        public async Task<IEnumerable<TaskReportDto>> TaskReport(TaskReportRequestDto taskReportDto, RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            try
            {
                string startDate = "";
                string endDate = "";
                int inCountIntervalMinute = 0;
                int inCountInterval = 0;
                _commonRepository = new CommonRepository();
                _dashBoardRepository = new DashBoardRepository();

                string decryptProjectId = await _commonRepository.Decrypt(taskReportDto.ProjectId, requestDetaildto.SecurityKey);
                string decryptModuleId = await _commonRepository.Decrypt(taskReportDto.ModuleId, requestDetaildto.SecurityKey);
                startDate = await _commonRepository.Decrypt(taskReportDto.StartDate, requestDetaildto.SecurityKey);
                Regex reg = new Regex("[\"]");
                if (!string.IsNullOrEmpty(startDate))
                {
                    string startDate2 = Convert.ToDateTime(startDate).ToString();
                    startDate = "TO_DATE('" + startDate2.Remove(startDate2.IndexOf(' ')) + " 00:00:00', 'MM/DD/YYYY HH24:MI:SS')";
                }
                endDate = await _commonRepository.Decrypt(taskReportDto.EndDate, requestDetaildto.SecurityKey);
                if (!string.IsNullOrEmpty(endDate))
                {
                    string endDate2 = Convert.ToDateTime(endDate).ToString();

                    endDate = "TO_DATE('" + endDate2.Remove(endDate2.IndexOf(' ')) + " 23:00:00', 'MM/DD/YYYY HH24:MI:SS')";

                }
                string decryptUserId = await _commonRepository.Decrypt(taskReportDto.EmployeeId, requestDetaildto.SecurityKey);

                _reportsRepository = new ReportsRepository();
                var report = await _reportsRepository.TaskReport(decryptProjectId,
                                                                decryptModuleId,
                                                                startDate,
                                                                endDate,
                                                                decryptUserId
                                                                );
                if (report != null)
                    foreach (var item in report)
                    {
                        _getEmployeeRepository = new EmployeeListRepository();
                        if (item.EMP_ID != null)
                        {
                            var assigneeDetails = await _getEmployeeRepository.GetEmployeeById(item.EMP_ID, "");
                            item.EMP_NAME = assigneeDetails.EMP_NAME;

                        }

                        var duration = await _dashBoardRepository.GetAssignIdByStartEndDuration("", item.ACTIVITY_ID);
                        if (duration.Count() > 0)
                        {
                            foreach (var itemDuration in duration)
                            {
                                if (itemDuration.END_TIME.ToString() != "1/1/0001 12:00:00 AM")
                                {
                                    var countInterval = itemDuration.END_TIME - itemDuration.START_TIME;
                                    inCountIntervalMinute = inCountIntervalMinute + countInterval.Minutes;
                                    inCountInterval = inCountInterval + countInterval.Hours;
                                }
                            }
                        }
                        inCountInterval = inCountInterval + (inCountIntervalMinute / 60);
                        inCountIntervalMinute = inCountIntervalMinute % 60;
                        if (item.COMPLETE_BY_ADMIN != "true")
                        {
                            item.DURATION_HOUR = inCountInterval;
                            item.DURATIONINMINUTES = inCountIntervalMinute;
                        }
                        if ( item.COMPLETE_BY_ADMIN == "true")
                        {
                            item.DURATION_HOUR =Convert.ToDateTime( item.ESTIMATED_TIME).Hour+ Convert.ToDateTime(item.EXTEND_TIME).Hour;
                            item.DURATIONINMINUTES = Convert.ToDateTime(item.ESTIMATED_TIME).Minute + Convert.ToDateTime(item.EXTEND_TIME).Minute;
                        }
                        inCountInterval = 0; inCountIntervalMinute = 0;
                    }
                var employeeDetailsMap = _mapper.Map<IEnumerable<TaskReportDto>>(report);

                strSysText = $"Get employees by { requestDetaildto.Username } ";
                return employeeDetailsMap;
            }
            catch (Exception ex)
            {
                strSysText = ex.Message.ToString();
                throw new Exception(strSysText);
            }
        }

        public async Task<IEnumerable<TaskReportDto>> ReportEmployeeActiveinactive(TaskReportRequestDto taskReportDto, RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            try
            {
                string startDate = "";
                string decryptEmpId = "";
                string decryptUserId = "";
                string endDate = "";
                DateTime? decryptStartDate = null;
                DateTime? decryptEndDate = null;
                _commonRepository = new CommonRepository();

                string decryptProjectId = await _commonRepository.Decrypt(taskReportDto.ProjectId, requestDetaildto.SecurityKey);
                string decryptModuleId = await _commonRepository.Decrypt(taskReportDto.ModuleId, requestDetaildto.SecurityKey);
                startDate = await _commonRepository.Decrypt(taskReportDto.StartDate, requestDetaildto.SecurityKey);
                Regex reg = new Regex("[\"]");
                if (!string.IsNullOrEmpty(startDate))
                {
                    decryptStartDate = Convert.ToDateTime(startDate);
                    string startDate2 = Convert.ToDateTime(startDate).ToString();
                    startDate = "TO_DATE('" + startDate2.Remove(startDate2.IndexOf(' ')) + " 00:00:00', 'MM/DD/YYYY HH24:MI:SS')";
                }
                endDate = await _commonRepository.Decrypt(taskReportDto.EndDate, requestDetaildto.SecurityKey);
                if (!string.IsNullOrEmpty(endDate))
                {
                    decryptEndDate = Convert.ToDateTime(endDate);
                    string endDate2 = Convert.ToDateTime(endDate).ToString();

                    endDate = "TO_DATE('" + endDate2.Remove(endDate2.IndexOf(' ')) + " 23:00:00', 'MM/DD/YYYY HH24:MI:SS')";

                }
                string decryptStatus = await _commonRepository.Decrypt(taskReportDto.ActivityStatus, requestDetaildto.SecurityKey);

                List<string> decryptEmpList = new List<string>();
                if (taskReportDto.EmpList.Count() > 0)
                {
                    foreach (var item in taskReportDto.EmpList)
                    {
                        if (item.emp != "")
                        {
                            decryptEmpId = await _commonRepository.Decrypt(item.emp, requestDetaildto.SecurityKey);
                            decryptEmpList.Add(decryptEmpId);
                        }
                    }
                }
                else
                {
                    _getEmployeeRepository = new EmployeeListRepository();
                    var employeeDetails = await _getEmployeeRepository.GetEmployees(requestDetaildto.UserId);
                    foreach (var item in employeeDetails)
                    {
                        decryptEmpList.Add(item.EMP_ID);
                    }
                }


                _reportsRepository = new ReportsRepository();
                var report = await _reportsRepository.ReportEmployeeActiveinactive(decryptProjectId,
                                                                decryptModuleId,
                                                                startDate,
                                                                endDate,
                                                                decryptUserId,
                                                                decryptEmpList
                                                                );
                List<TaskReport> activeInactiveTaskReport = new List<TaskReport>();
                if (report != null)
                {
                    foreach (var item in report)
                    {
                        _getEmployeeRepository = new EmployeeListRepository();
                        if (!string.IsNullOrEmpty(item.EMP_ID))
                        {
                            var assigneeDetails = await _getEmployeeRepository.GetEmployeeById(item.EMP_ID, "");
                            item.EMP_NAME = assigneeDetails.EMP_NAME;
                        }
                        if (!string.IsNullOrEmpty(item.CREATE_BY))
                        {
                            var assigneeDetails = await _getEmployeeRepository.GetEmployeeById("", item.CREATE_BY);
                            item.TASKCREATOR_NAME = assigneeDetails.EMP_NAME;
                        }
                        if (item.ACTIVITY_STATUS != "Not-Assigned")
                        {
                            item.ACTIVITY_STATUS = "Assigned";
                        }
                    }

                    foreach (var item in report)
                    {
                        var match = activeInactiveTaskReport.FirstOrDefault(x => x.EMP_ID == item.EMP_ID);
                        if (decryptStatus == "0")
                        {
                            var y = Convert.ToDateTime(decryptStartDate).Date;
                            var z = Convert.ToDateTime(decryptEndDate).Date;
                            var x = Convert.ToDateTime(item.ASSIGNEE_DATE).Date;

                            if (match == null || (y <= x && z >= x))
                            {
                                activeInactiveTaskReport.Add(item);
                            }
                        }
                        if (decryptStatus == item.ACTIVITY_STATUS && match == null)
                        {
                            activeInactiveTaskReport.Add(item);
                        }
                    }
                }

                var employeeDetailsMap = _mapper.Map<IEnumerable<TaskReportDto>>(activeInactiveTaskReport);

                strSysText = $"Get employees by { requestDetaildto.Username } ";
                return employeeDetailsMap;
            }
            catch (Exception ex)
            {
                strSysText = ex.Message.ToString();
                throw new Exception(strSysText);
            }
        }



        public async Task<IEnumerable<TaskReportDto>> PersonnelActivitySummary(TaskReportRequestDto taskReportDto, RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            try
            {
                string decryptEmpId = "";
                string startDate = "";
                string endDate = "";
                int count = 0;
                int countMinute = 0;
                //DateTime? decryptStartDate = null;
                _dashBoardRepository = new DashBoardRepository();
                //DateTime? decryptEndDate = null;
                _commonRepository = new CommonRepository();

                startDate = await _commonRepository.Decrypt(taskReportDto.StartDate, requestDetaildto.SecurityKey);
                Regex reg = new Regex("[\"]");
                if (!string.IsNullOrEmpty(startDate))
                {
                    string startDate2 = Convert.ToDateTime(startDate).ToString();
                    startDate = "TO_DATE('" + startDate2.Remove(startDate2.IndexOf(' ')) + " 00:00:00', 'MM/DD/YYYY HH24:MI:SS')";
                }
                endDate = await _commonRepository.Decrypt(taskReportDto.EndDate, requestDetaildto.SecurityKey);
                if (!string.IsNullOrEmpty(endDate))
                {
                    string endDate2 = Convert.ToDateTime(endDate).ToString();

                    endDate = "TO_DATE('" + endDate2.Remove(endDate2.IndexOf(' ')) + " 23:00:00', 'MM/DD/YYYY HH24:MI:SS')";

                }
                //string decryptUserId = await _commonRepository.Decrypt(taskReportDto.EmployeeId, requestDetaildto.SecurityKey);
                List<string> decryptEmpList = new List<string>();
                List<TaskReport> reportList = new List<TaskReport>();
                if (taskReportDto.EmpList.Count() > 0)
                {
                    foreach (var item in taskReportDto.EmpList)
                    {
                        if (item.emp != "")
                        {
                            decryptEmpId = await _commonRepository.Decrypt(item.emp, requestDetaildto.SecurityKey);
                            decryptEmpList.Add(decryptEmpId);
                        }
                    }
                }
                else
                {
                    _getEmployeeRepository = new EmployeeListRepository();
                    var employeeDetails = await _getEmployeeRepository.GetEmployees(requestDetaildto.UserId);
                    foreach (var item in employeeDetails)
                    {
                        decryptEmpList.Add(item.EMP_ID);
                    }
                }
                _reportsRepository = new ReportsRepository();
                var report = await _reportsRepository.PersonnelActivitySummary(startDate, endDate, decryptEmpList);
                foreach (var item in report)
                {
                    _getEmployeeRepository = new EmployeeListRepository();
                    if (item != null)
                    {
                        var assigneeDetails = await _getEmployeeRepository.GetEmployeeById(item.EMP_ID, "");
                        item.EMP_NAME = assigneeDetails.EMP_NAME;

                        var employeeTaskEstimData = await _reportsRepository.EstimatedTimeTaskListId(item.EMP_ID, startDate, endDate);
                        foreach (var itemEmployeeEstim in employeeTaskEstimData)
                        {
                            if (!string.IsNullOrEmpty(itemEmployeeEstim.TASKLIST_ID))
                            {
                                var chkActive = await _reportsRepository.TaskTempActivities(itemEmployeeEstim.TASKLIST_ID);
                                if (itemEmployeeEstim.STATUS_LIST_ID == "200623010015000002" && chkActive.Count() == 0)
                                {
                                    var doneTaskDuration = await _reportsRepository.DoneTask(itemEmployeeEstim.TASKLIST_ID);
                                    count = count + Convert.ToDateTime(doneTaskDuration.ESTIMATED_TIME).Hour;
                                    countMinute = countMinute + Convert.ToDateTime(doneTaskDuration.ESTIMATED_TIME).Minute;
                                }
                                else
                                {
                                    var taskduration = await _dashBoardRepository.GetAssignIdByStartEndDuration(itemEmployeeEstim.TASK_ASSIGNEE_ID, "");
                                    if (taskduration.Count() > 0)
                                    {
                                        foreach (var itemDuration in taskduration)
                                        {
                                            if (itemDuration.START_TIME.ToString() == "1/1/0001 12:00:00 AM" || itemDuration.END_TIME.ToString() == "1/1/0001 12:00:00 AM")
                                            {
                                            }
                                            else
                                            {
                                                var countInterval = itemDuration.END_TIME - itemDuration.START_TIME;
                                                countMinute = countMinute + countInterval.Minutes;

                                                count = count + countInterval.Hours;
                                            }
                                        }
                                    }
                                }
                                count = count + (countMinute / 60);
                                countMinute = countMinute % 60;
                            }

                        }
                        item.DURATION_OF = Convert.ToString(count) + ":" + Convert.ToString(countMinute);
                        count = 0;
                        countMinute = 0;
                        reportList.Add(item);
                    }
                }

                var employeeDetailsMap = _mapper.Map<IEnumerable<TaskReportDto>>(reportList);

                strSysText = $"Get employees by { requestDetaildto.Username } ";
                return employeeDetailsMap;
            }
            catch (Exception ex)
            {
                strSysText = ex.Message.ToString();
                throw new Exception(strSysText);
            }
        }

        public async Task<IEnumerable<TaskReportDto>> PersonnelActivitySummaryDetails(TaskReportRequestDto taskReportDto, RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            try
            {

                string startDate = "";
                string endDate = "";
                string decryptEmpId = ""; string taskEstimate = "";
                int count = 0;
                int countMinute = 0;
                _commonRepository = new CommonRepository();
                _dashBoardRepository = new DashBoardRepository();
                startDate = await _commonRepository.Decrypt(taskReportDto.StartDate, requestDetaildto.SecurityKey);
                Regex reg = new Regex("[\"]");
                if (!string.IsNullOrEmpty(startDate))
                {
                    string startDate2 = Convert.ToDateTime(startDate).ToString();
                    startDate = "TO_DATE('" + startDate2.Remove(startDate2.IndexOf(' ')) + " 00:00:00', 'MM/DD/YYYY HH24:MI:SS')";
                }
                endDate = await _commonRepository.Decrypt(taskReportDto.EndDate, requestDetaildto.SecurityKey);
                if (!string.IsNullOrEmpty(endDate))
                {
                    string endDate2 = Convert.ToDateTime(endDate).ToString();

                    endDate = "TO_DATE('" + endDate2.Remove(endDate2.IndexOf(' ')) + " 23:00:00', 'MM/DD/YYYY HH24:MI:SS')";

                }
                decryptEmpId = await _commonRepository.Decrypt(taskReportDto.EmployeeId, requestDetaildto.SecurityKey);

                List<string> decryptEmpList = new List<string>();
                List<TaskReport> reportList = new List<TaskReport>();
                _reportsRepository = new ReportsRepository();
                var report = await _reportsRepository.PersonnelActivitySummaryDetails(startDate, endDate, decryptEmpId);
                foreach (var item in report)
                {
                    _getEmployeeRepository = new EmployeeListRepository();
                    if (item.EMP_ID != null)
                    {
                        var assigneeDetails = await _getEmployeeRepository.GetEmployeeById(item.EMP_ID, "");
                        item.EMP_NAME = assigneeDetails.EMP_NAME;

                        if (!string.IsNullOrEmpty(item.TASKLIST_ID))
                        {
                            var chkActive = await _reportsRepository.TaskTempActivities(item.TASKLIST_ID);
                            if (item.STATUS_LIST_ID == "200623010015000002" && chkActive.Count() == 0)
                            {
                                var doneTaskDuration = await _reportsRepository.DoneTask(item.TASKLIST_ID);
                                count = count + Convert.ToDateTime(doneTaskDuration.ESTIMATED_TIME).Hour;
                                countMinute = countMinute + Convert.ToDateTime(doneTaskDuration.ESTIMATED_TIME).Minute;
                            }
                            else
                            {
                                var taskduration = await _dashBoardRepository.GetAssignIdByStartEndDuration(item.TASK_ASSIGNEE_ID, "");
                                if (taskduration.Count() > 0)
                                {
                                    foreach (var itemDuration in taskduration)
                                    {
                                        if (itemDuration.START_TIME.ToString() == "1/1/0001 12:00:00 AM" || itemDuration.END_TIME.ToString() == "1/1/0001 12:00:00 AM")
                                        {
                                        }
                                        else
                                        {
                                            var countInterval = itemDuration.END_TIME - itemDuration.START_TIME;
                                            countMinute = countMinute + countInterval.Minutes;

                                            count = count + countInterval.Hours;
                                        }
                                    }
                                }
                            }
                            count = count + (countMinute / 60);
                            countMinute = countMinute % 60;
                        }
                        item.DURATION_OF = Convert.ToString(count) + ":" + Convert.ToString(countMinute);
                        count = 0;
                        countMinute = 0;
                        taskEstimate = "";
                        reportList.Add(item);
                    }

                }
                var employeeDetailsMap = _mapper.Map<IEnumerable<TaskReportDto>>(reportList);

                strSysText = $"Get employees by { requestDetaildto.Username } ";
                return employeeDetailsMap;
            }
            catch (Exception ex)
            {
                strSysText = ex.Message.ToString();
                throw new Exception(strSysText);
            }
        }

    }
}
