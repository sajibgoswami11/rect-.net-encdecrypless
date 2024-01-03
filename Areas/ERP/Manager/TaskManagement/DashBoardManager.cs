using AutoMapper;
using BizWebAPI.Areas.ERP.Common;
using BizWebAPI.Areas.ERP.Dtos;
using BizWebAPI.Areas.ERP.Dtos.TaskManagement;
using BizWebAPI.Areas.ERP.Interfaces.TaskManagement;
using BizWebAPI.Areas.ERP.Models.TaskManagement;
using BizWebAPI.Areas.ERP.Repository;
using BizWebAPI.Areas.ERP.Repository.TaskManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.Manager.TaskManagement
{
    public class DashBoardManager : IDashBoard
    {
        private CommonRepository _commonRepository;
        private DashBoardRepository _dashBoardRepository;
        private TaskHelperRepository _taskManagementHelper;
        private TaskAssignRepository _taskAssignRepository;
        private TaskNotificationRepoitory _taskNotificationRepository;
        private EmployeeListRepository _getEmployeeRepository;
        private readonly IMapper _mapper;

        public DashBoardManager(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<string> CommonMessage()
        {
            _commonRepository = new CommonRepository();
            return await _commonRepository.CommonMessage();
        }

        public async Task<IEnumerable<TaskListDto>> GetTaskList(RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            try
            {
                _dashBoardRepository = new DashBoardRepository();
                var taskList = await _dashBoardRepository.GetTaskList(requestDetaildto.UserId);
                var taskLists = _mapper.Map<IEnumerable<TaskListDto>>(taskList);

                strSysText = requestDetaildto.Username + " has successfully fetched TaskList";
                return taskLists;
            }
            catch (Exception ex)
            {
                strSysText = ex.Message.ToString();
                throw new Exception(strSysText);
            }
            finally
            {
                _commonRepository = new CommonRepository();
                await _commonRepository.SystemAudit(requestDetaildto, strSysText);
            }
        }

        public async Task<IEnumerable<TaskForwardDto>> GetTaskForwardList(RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            try
            {
                _dashBoardRepository = new DashBoardRepository();
                var taskForwardList = await _dashBoardRepository.GetTaskForwardList();
                var taskForwardListMap = _mapper.Map<IEnumerable<TaskForwardDto>>(taskForwardList);

                strSysText = requestDetaildto.Username + " has successfully forwarded Task ";
                return taskForwardListMap;
            }
            catch (Exception ex)
            {
                strSysText = ex.Message.ToString();
                throw new Exception(strSysText);
            }
            finally
            {
                _commonRepository = new CommonRepository();
                await _commonRepository.SystemAudit(requestDetaildto, strSysText);
            }
        }
        //// get summary  on modal by clicking project row body 
        public async Task<TaskSummaryInfoDto> GetSummary(DashBoardDto dashBoardDto, RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            int inCountIntervalMinute = 0;
            int inCountInterval = 0;
            try
            {
                _taskManagementHelper = new TaskHelperRepository();
                _dashBoardRepository = new DashBoardRepository();
                _commonRepository = new CommonRepository();
                TaskSummaryInfoDto projectInfo = new TaskSummaryInfoDto();
                var decryptProjectId = await _commonRepository.Decrypt(dashBoardDto.ProjectId, requestDetaildto.SecurityKey);
                var detailsByProject = await _dashBoardRepository.GetTaskDtailsByProjectId(decryptProjectId, requestDetaildto.UserId);
                foreach (var item in detailsByProject)
                {
                    if (!string.IsNullOrEmpty(item.TASK_ASSIGNEE_ID))
                    {
                        var assingPerson = await _dashBoardRepository.GetEmployeeByAssign(item.TASK_ASSIGNEE_ID);
                        item.EMP_NAME = assingPerson.FirstOrDefault().EMP_NAME;
                        var duration = await _dashBoardRepository.GetAssignIdByStartEndDuration(item.TASK_ASSIGNEE_ID, item.ACTIVITY_ID);
                        if (duration.Count() > 0)
                            foreach (var itemDuration in duration)
                            {
                                if (itemDuration.END_TIME.ToString() != "1/1/0001 12:00:00 AM")
                                {
                                    var countInterval = itemDuration.END_TIME - itemDuration.START_TIME;
                                    inCountIntervalMinute = inCountIntervalMinute + countInterval.Minutes;
                                    inCountInterval = inCountInterval + countInterval.Hours;
                                }
                            }
                        inCountInterval = inCountInterval + (inCountIntervalMinute / 60);
                        inCountIntervalMinute = inCountIntervalMinute % 60;
                        item.Duration = inCountInterval;
                        item.TimeDurationInMinutes = inCountIntervalMinute;
                        inCountInterval = 0; inCountIntervalMinute = 0;
                    }
                }


                projectInfo.DashboardPercentage = _mapper.Map<IEnumerable<DashBoardPercentageDto>>(detailsByProject);

                strSysText = requestDetaildto.Username + " has successfully fetched record";
                return projectInfo;
            }
            catch (Exception ex)
            {
                strSysText = ex.Message.ToString();
                throw new Exception(strSysText);
            }
            finally
            {
                _commonRepository = new CommonRepository();
                await _commonRepository.SystemAudit(requestDetaildto, strSysText);
            }
        }


        public async Task<TaskSummaryInfoDto> GetSummaryByLogin(RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            try
            {
                _taskManagementHelper = new TaskHelperRepository();
                _taskAssignRepository = new TaskAssignRepository();
                var employee = await _taskManagementHelper.GetEmployeeByUserId(requestDetaildto.UserId);
                string empId = TaskAppHelper.GetFieldValueById(employee, "EMP_ID");

                TaskSummaryInfoDto ProjectInfo = new TaskSummaryInfoDto();
                var totalProjectDetails = await _taskManagementHelper.GetProjectInfo(empId, "");
                var totalProjectDetailsMap = _mapper.Map<IEnumerable<TaskProjectDto>>(totalProjectDetails);
                ProjectInfo.ProjectDetails = totalProjectDetailsMap;

                var totalModuleDetails = await _taskManagementHelper.GetModuleInfo(empId);
                ProjectInfo.ModuleDetails = _mapper.Map<IEnumerable<TaskModuleDto>>(totalModuleDetails);

                var totalTaskDetails = await _taskAssignRepository.GetTaskAssignList(requestDetaildto.UserId);
                ProjectInfo.TaskAssignDetails = _mapper.Map<IEnumerable<TaskAssignDto>>(totalTaskDetails);

                _dashBoardRepository = new DashBoardRepository();
                var totalTaskListDetails = await _dashBoardRepository.GetTaskList(requestDetaildto.UserId);
                ProjectInfo.TaskListDetails = _mapper.Map<IEnumerable<TaskCreateAssignDto>>(totalTaskListDetails);

                return ProjectInfo;
            }
            catch (Exception ex)
            {
                strSysText = ex.Message.ToString();
                throw new Exception(strSysText);
            }
            finally
            {
                _commonRepository = new CommonRepository();
                await _commonRepository.SystemAudit(requestDetaildto, strSysText);
            }
        }

        // projectwise counting
        public async Task<IEnumerable<DashBoardPercentageDto>> GetProjectProgress(RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            try
            {
                _taskManagementHelper = new TaskHelperRepository();
                var employee = await _taskManagementHelper.GetEmployeeByUserId(requestDetaildto.UserId);
                string empId = TaskAppHelper.GetFieldValueById(employee, "EMP_ID");

                _dashBoardRepository = new DashBoardRepository();
                var projectDetails = await _dashBoardRepository.GetProjectByEmployee(empId);
                var projectDetailsMap = _mapper.Map<IEnumerable<TaskProjectDto>>(projectDetails);


                List<DashBoardPercentageDto> DashBoardPercentage = new List<DashBoardPercentageDto>();

                foreach (var item in projectDetailsMap)
                {
                    var progress = await _dashBoardRepository.GetProjectProgress(item.ProjectId, empId);
                    if (progress.Project_Name != null)
                    {
                        progress.Project_Id = item.ProjectId;
                        if (string.IsNullOrEmpty(progress.Remaining_Task))
                        {
                            progress.Remaining_Task = "0";
                        }
                        if (progress.Complete_Task == null)
                        {
                            progress.Complete_Task = "0";
                        }
                        var progressDtls = _mapper.Map<DashBoardPercentageDto>(progress);
                        DashBoardPercentage.Add(progressDtls);
                    }
                }

                strSysText = requestDetaildto.Username + " has successfully forwarded Task ";
                return DashBoardPercentage;
            }
            catch (Exception ex)
            {
                strSysText = ex.Message.ToString();
                throw new Exception(strSysText);
            }
            finally
            {
                _commonRepository = new CommonRepository();
                await _commonRepository.SystemAudit(requestDetaildto, strSysText);
            }
        }

        // extra 
        public async Task<TaskProjectProgressSummaryDto> GetDateRangeProjectProgressSummary(DateRangeDto dateRange, RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            try
            {
                _taskManagementHelper = new TaskHelperRepository();
                var employee = await _taskManagementHelper.GetEmployeeByUserId(requestDetaildto.UserId);
                string empId = TaskAppHelper.GetFieldValueById(employee, "EMP_ID");

                _commonRepository = new CommonRepository();
                string decryptStartDate = await _commonRepository.Decrypt(dateRange.StartDate, requestDetaildto.SecurityKey);
                string decryptEndDate = await _commonRepository.Decrypt(dateRange.EndDate, requestDetaildto.SecurityKey);

                _dashBoardRepository = new DashBoardRepository();
                var ProjectProgressSummary = await _dashBoardRepository.GetDateRangeProjectProgressSummary(decryptStartDate, decryptEndDate, empId);
                if (string.IsNullOrEmpty(ProjectProgressSummary.Complete_Task))
                    ProjectProgressSummary.Complete_Task = "0";
                if (string.IsNullOrEmpty(ProjectProgressSummary.Closing_Ratio))
                    ProjectProgressSummary.Closing_Ratio = "0";
                if (string.IsNullOrEmpty(ProjectProgressSummary.Remaining_Task))
                    ProjectProgressSummary.Remaining_Task = "0";
                if (string.IsNullOrEmpty(ProjectProgressSummary.Total_Task))
                    ProjectProgressSummary.Total_Task = "0";
                if (string.IsNullOrEmpty(ProjectProgressSummary.Active))
                    ProjectProgressSummary.Active = "0";
                if (string.IsNullOrEmpty(ProjectProgressSummary.In_Progress))
                    ProjectProgressSummary.In_Progress = "0";

                var ProjectProgressSummaryMap = _mapper.Map<TaskProjectProgressSummaryDto>(ProjectProgressSummary);

                strSysText = requestDetaildto.Username + " has successfully ProjectProgressSummary fetch ";
                return ProjectProgressSummaryMap;
            }
            catch (Exception ex)
            {
                strSysText = ex.Message.ToString();
                throw new Exception(strSysText);
            }
            finally
            {
                _commonRepository = new CommonRepository();
                await _commonRepository.SystemAudit(requestDetaildto, strSysText);
            }
        }

        public async Task<IEnumerable<DashboardProjectDetailsDto>> DashboardOnProjectCountClickGetDetails(RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            try
            {
                List<DashboardProjectDetailsDto> dashOnProjectCountClickGetDetailsDto = new List<DashboardProjectDetailsDto>();

                _taskManagementHelper = new TaskHelperRepository();
                var employee = await _taskManagementHelper.GetEmployeeByUserId(requestDetaildto.UserId);
                string empId = TaskAppHelper.GetFieldValueById(employee, "EMP_ID");

                _dashBoardRepository = new DashBoardRepository();
                var projectDetails = await _dashBoardRepository.GetProjectByEmployee(empId);

                foreach (var item in projectDetails)
                {
                    var getDataOnProjectCountClick = await _taskManagementHelper.DashboardOnProjectCountClickGetDetails(item.PROJECT_ID);
                    if (getDataOnProjectCountClick == null)
                    {
                    }
                    else
                    {
                        var creatorDetails = await _taskManagementHelper.GetEmployeeByUserId(getDataOnProjectCountClick.CreateBy);
                        getDataOnProjectCountClick.CreateBy = creatorDetails.EMP_NAME;
                        dashOnProjectCountClickGetDetailsDto.Add(getDataOnProjectCountClick);
                    }

                }

                return dashOnProjectCountClickGetDetailsDto;
            }
            catch (Exception ex)
            {
                strSysText = ex.Message.ToString();
                throw new Exception(strSysText);
            }
            finally
            {
                _commonRepository = new CommonRepository();
                await _commonRepository.SystemAudit(requestDetaildto, strSysText);
            }
        }

        public async Task<IEnumerable<DashboardModuleDetailsDto>> DashboardOnModuleCountClickGetDetails(RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            try
            {
                List<DashboardModuleDetailsDto> dashOnModuleCountClickGetDetailsDto = new List<DashboardModuleDetailsDto>();

                _taskManagementHelper = new TaskHelperRepository();
                var employee = await _taskManagementHelper.GetEmployeeByUserId(requestDetaildto.UserId);
                string empId = TaskAppHelper.GetFieldValueById(employee, "EMP_ID");

                _dashBoardRepository = new DashBoardRepository();
                var moduleDetails = await _dashBoardRepository.GetModuleByEmployee(empId);

                foreach (var item in moduleDetails)
                {
                    var getDataOnModuleCountClick = await _taskManagementHelper.DashboardOnModuleCountClickGetDetails(item.MODULE_ID);
                    if (getDataOnModuleCountClick == null)
                    {
                    }
                    else
                    {
                        var creatorDetails = await _taskManagementHelper.GetEmployeeByUserId(getDataOnModuleCountClick.CreateBy);
                        getDataOnModuleCountClick.CreateBy = creatorDetails.EMP_NAME;
                        dashOnModuleCountClickGetDetailsDto.Add(getDataOnModuleCountClick);
                    }

                }

                return dashOnModuleCountClickGetDetailsDto;
            }
            catch (Exception ex)
            {
                strSysText = ex.Message.ToString();
                throw new Exception(strSysText);
            }
            finally
            {
                _commonRepository = new CommonRepository();
                await _commonRepository.SystemAudit(requestDetaildto, strSysText);
            }
        }

        public async Task<IEnumerable<DashboardAssignDetailsDto>> DashboardOnTaskAssignCountClickGetDetails(RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            int inCountInterval = 0;
            int inCountIntervalMinute = 0;
            bool timeExceed = false;
            int replacedHour = 0; int replacesMinute = 0;
            try
            {
                _taskNotificationRepository = new TaskNotificationRepoitory();

                List<DashboardAssignDetailsDto> dashOnTaskAssignCountClickGetDetailsDto = new List<DashboardAssignDetailsDto>();

                _taskManagementHelper = new TaskHelperRepository();
                var employee = await _taskManagementHelper.GetEmployeeByUserId(requestDetaildto.UserId);
                string empId = TaskAppHelper.GetFieldValueById(employee, "EMP_ID");
                _getEmployeeRepository = new EmployeeListRepository();
                _dashBoardRepository = new DashBoardRepository();
                var assignDetails = await _dashBoardRepository.GetListByEmployee(empId);

                foreach (var item in assignDetails)
                {
                    List<DateTime> listStartTime = new List<DateTime>();
                    List<DateTime> listEndTime = new List<DateTime>();
                    var getDataOnTaskAssignCountClick = await _taskManagementHelper.DashboardOnTaskAssignCountClickGetDetails(item.TASKLIST_ID);
                    if (getDataOnTaskAssignCountClick != null)
                    {
                        var assigneeDetails = await _getEmployeeRepository.GetEmployeeById(getDataOnTaskAssignCountClick.AssignTo, "");
                        getDataOnTaskAssignCountClick.AssignToName = assigneeDetails.EMP_NAME;
                        var duration = await _dashBoardRepository.GetAssignIdByStartEndDuration(item.TASK_ASSIGNEE_ID, "");
                        if (duration.Count() > 0)
                        {
                            foreach (var itemDuration in duration)
                            {
                                if (itemDuration.START_TIME.ToString() == "1/1/0001 12:00:00 AM" || itemDuration.END_TIME.ToString() == "1/1/0001 12:00:00 AM")
                                {
                                    if (itemDuration.START_TIME.ToString() != "1/1/0001 12:00:00 AM" && itemDuration.END_TIME.ToString() == "1/1/0001 12:00:00 AM")
                                    {
                                        getDataOnTaskAssignCountClick.EndTime = null;
                                        listStartTime.Add(itemDuration.START_TIME);
                                    }
                                    if (itemDuration.START_TIME.ToString() == "1/1/0001 12:00:00 AM" && itemDuration.END_TIME.ToString() != "1/1/0001 12:00:00 AM")
                                    {
                                        getDataOnTaskAssignCountClick.StartTime = null;
                                        listStartTime.Add(itemDuration.END_TIME);
                                    }
                                }
                                else
                                {
                                    listStartTime.Add(itemDuration.START_TIME);
                                    listEndTime.Add(itemDuration.END_TIME);
                                    var countInterval = itemDuration.END_TIME - itemDuration.START_TIME;
                                    inCountIntervalMinute = inCountIntervalMinute + countInterval.Minutes;

                                    inCountInterval = inCountInterval + countInterval.Hours;
                                }

                            }
                            inCountInterval = inCountInterval + (inCountIntervalMinute / 60);
                            inCountIntervalMinute = inCountIntervalMinute % 60;
                            getDataOnTaskAssignCountClick.StartTime = listStartTime.Min(date1 => date1);
                            if (listEndTime.Count() > 0)
                            {
                                getDataOnTaskAssignCountClick.EndTime = listEndTime.Max(date1 => date1);
                            }
                        }
                        else
                        {
                            getDataOnTaskAssignCountClick.StartTime = null;
                            getDataOnTaskAssignCountClick.EndTime = null;
                        }

                        getDataOnTaskAssignCountClick.TimeDuration = inCountInterval;
                        getDataOnTaskAssignCountClick.TimeDurationInMinutes = inCountIntervalMinute;
                        inCountInterval = 0;
                        inCountIntervalMinute = 0;

                        DateTime estimateTimeDate;
                        DateTime extendTimespan;

                        if (DateTime.TryParse(getDataOnTaskAssignCountClick.EstimatedTime, out estimateTimeDate))
                        {
                            var dataTaskEstimateTime = await _taskNotificationRepository.TaskListIdEstimateTime("", "", item.TASK_ASSIGNEE_ID);
                            if (dataTaskEstimateTime != null)
                            {
                                foreach (var itemdataTaskEstim in dataTaskEstimateTime)
                                {
                                    if (itemdataTaskEstim.EXTEND_TIME != null)
                                    {
                                        if (DateTime.TryParse(itemdataTaskEstim.EXTEND_TIME, out extendTimespan))
                                        {
                                            replacedHour = estimateTimeDate.Hour + extendTimespan.Hour;
                                            replacesMinute = estimateTimeDate.Minute + extendTimespan.Minute;
                                        }
                                    }
                                    else
                                    {
                                        replacedHour = estimateTimeDate.Hour;
                                        replacesMinute = estimateTimeDate.Minute;
                                    }
                                }
                            }
                            else
                            {
                                replacedHour = estimateTimeDate.Hour;
                                replacesMinute = estimateTimeDate.Minute;
                            }
                            replacedHour = replacedHour + (replacesMinute / 60);
                            replacesMinute = replacesMinute % 60;
                            if ((replacedHour) < getDataOnTaskAssignCountClick.TimeDuration)
                            {
                                timeExceed = true;
                                getDataOnTaskAssignCountClick.TimeExceed = timeExceed;
                            }
                            else
                            {
                                if ((replacesMinute) < getDataOnTaskAssignCountClick.TimeDurationInMinutes && (replacedHour) == getDataOnTaskAssignCountClick.TimeDuration)
                                {
                                    timeExceed = true;
                                    getDataOnTaskAssignCountClick.TimeExceed = timeExceed;
                                }
                            }
                        }
                        else
                        {
                            if (Convert.ToInt32(getDataOnTaskAssignCountClick.EstimatedTime) < getDataOnTaskAssignCountClick.TimeDuration)
                            {
                                timeExceed = true;
                                getDataOnTaskAssignCountClick.TimeExceed = timeExceed;
                            }
                        }

                        getDataOnTaskAssignCountClick.EstimatedTime = (replacedHour).ToString() + ":" + replacesMinute.ToString();
                        replacedHour = 0; replacesMinute = 0;
                        dashOnTaskAssignCountClickGetDetailsDto.Add(getDataOnTaskAssignCountClick);
                    }

                }

                return dashOnTaskAssignCountClickGetDetailsDto;
            }
            catch (Exception ex)
            {
                strSysText = ex.Message.ToString();
                throw new Exception(strSysText);
            }
        }

        public async Task<IEnumerable<DashboardCompleteDetailsDto>> DashboardOnTaskCompleteCountClickGetDetails(RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            int inCountInterval = 0;
            int inCountIntervalMinute = 0;
            try
            {

                List<DashboardCompleteDetailsDto> dashOnTaskCompleteCountClickGetDetailsDto = new List<DashboardCompleteDetailsDto>();
                _taskManagementHelper = new TaskHelperRepository();
                var employee = await _taskManagementHelper.GetEmployeeByUserId(requestDetaildto.UserId);
                string empId = TaskAppHelper.GetFieldValueById(employee, "EMP_ID");
                _getEmployeeRepository = new EmployeeListRepository();
                _dashBoardRepository = new DashBoardRepository();
                var assignDetails = await _dashBoardRepository.GetListByEmployee(empId);

                foreach (var item in assignDetails)
                {
                    List<DateTime> listStartTime = new List<DateTime>();
                    List<DateTime> listEndTime = new List<DateTime>();
                    var getDataOnTaskCompleteCountClick = await _taskManagementHelper.DashboardOnTaskCompleteCountClickGetDetails(item.TASKLIST_ID);
                    if (getDataOnTaskCompleteCountClick != null)
                    {
                        var assigneeDetails = await _getEmployeeRepository.GetEmployeeById(getDataOnTaskCompleteCountClick.AssignTo, "");
                        getDataOnTaskCompleteCountClick.AssignTo = assigneeDetails.EMP_NAME;
                        var duration = await _dashBoardRepository.GetAssignIdByStartEndDuration(item.TASK_ASSIGNEE_ID, "");
                        if (duration.Count() > 0)
                        {
                            foreach (var itemDuration in duration)
                            {
                                listStartTime.Add(itemDuration.START_TIME);
                                listEndTime.Add(itemDuration.END_TIME);
                                var countInterval = itemDuration.END_TIME - itemDuration.START_TIME;
                                inCountIntervalMinute = inCountIntervalMinute + countInterval.Minutes;

                                inCountInterval = inCountInterval + countInterval.Hours;
                            }
                            inCountInterval = inCountInterval + (inCountIntervalMinute / 60);
                            inCountIntervalMinute = inCountIntervalMinute % 60;
                            getDataOnTaskCompleteCountClick.StartTime = listStartTime.Min(date1 => date1);
                            getDataOnTaskCompleteCountClick.EndTime = listEndTime.Max(date1 => date1);
                        }
                        else
                        {
                            getDataOnTaskCompleteCountClick.StartTime = null;
                            getDataOnTaskCompleteCountClick.EndTime = null;
                        }
                        DateTime estimateTimeDate;

                        if (inCountInterval == 0 && inCountIntervalMinute == 0)
                        {
                            if (DateTime.TryParse(getDataOnTaskCompleteCountClick.EstimatedTime, out estimateTimeDate))
                            {
                                getDataOnTaskCompleteCountClick.TimeDuration = Convert.ToDateTime(getDataOnTaskCompleteCountClick.EstimatedTime).Hour;
                                getDataOnTaskCompleteCountClick.TimeDurationInMinutes = Convert.ToDateTime(getDataOnTaskCompleteCountClick.EstimatedTime).Minute;
                            }
                        }
                        else
                        {
                            getDataOnTaskCompleteCountClick.TimeDuration = inCountInterval;
                            getDataOnTaskCompleteCountClick.TimeDurationInMinutes = inCountIntervalMinute;
                        }
                        inCountInterval = 0;
                        inCountIntervalMinute = 0;
                        dashOnTaskCompleteCountClickGetDetailsDto.Add(getDataOnTaskCompleteCountClick);
                    }
                }
                return dashOnTaskCompleteCountClickGetDetailsDto;
            }
            catch (Exception ex)
            {
                strSysText = ex.Message.ToString();
                throw new Exception(strSysText);
            }
        }

    }
}
