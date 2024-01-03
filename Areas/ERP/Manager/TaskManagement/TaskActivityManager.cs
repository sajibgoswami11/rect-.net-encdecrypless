using BizWebAPI.Areas.ERP.Dtos;
using BizWebAPI.Areas.ERP.Dtos.TaskManagement;
using BizWebAPI.Areas.ERP.Interfaces.TaskManagement;
using BizWebAPI.Areas.ERP.Repository;
using BizWebAPI.Areas.ERP.Repository.TaskManagement;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using BizWebAPI.Common;
using System.IO;
using BizWebAPI.Areas.ERP.Common;
using System.Linq;

namespace BizWebAPI.Areas.ERP.Manager.TaskManagement
{
    public class TaskActivityManager : ITaskActivity
    {
        private TaskNotificationRepoitory _taskNotificationRepository;
        private EmployeeListRepository _getEmployeeRepository;
        private DashBoardRepository _dashBoardRepository;
        private TaskHelperRepository _taskManagementHelper;
        private TaskActivityRepository taskTaskActivityRepository;
        private CommonRepository _commonRepository;
        public ServiceHandler objServiceHandler = new ServiceHandler();
        private readonly IMapper _mapper;

        public TaskActivityManager(IMapper mapper)
        {
            _mapper = mapper;
        }
        public async Task<string> CommonMessage()
        {
            _commonRepository = new CommonRepository();
            return await _commonRepository.CommonMessage();
        }

        public async Task<string> CreateTaskActivity(TaskActivityRequestDto dto, RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            string returnFilePath = "";
            string byteDataCheck = "";
            byte[] fileSource = null;
            int imageName = 0;
            try
            {
                _commonRepository = new CommonRepository();
                //string decryptStartTime = await _commonRepository.Decrypt(dto.StartTime, requestDetaildto.SecurityKey);
                //string decryptEndTime = await _commonRepository.Decrypt(dto.EndTime, requestDetaildto.SecurityKey);
                string decryptRemarks = await _commonRepository.Decrypt(dto.Remarks, requestDetaildto.SecurityKey);
                //string decryptTaskStatusListId = await _commonRepository.Decrypt(dto.TaskStatusListId, requestDetaildto.SecurityKey);
                string decryptTaskAssigneId = await _commonRepository.Decrypt(dto.TaskAssigneeId, requestDetaildto.SecurityKey);
                string decryptEmpId = await _commonRepository.Decrypt(dto.EmpId, requestDetaildto.SecurityKey);

                objServiceHandler = new ServiceHandler();
                List<string> returnFilePathList = new List<string>();
                if (dto.ActivityImageSource != null)
                {
                    foreach (var base64String in dto.ActivityImageSource)
                    {
                        imageName++;
                        byteDataCheck = base64String.Substring(0, base64String.IndexOf(';'));
                        var fileData = base64String.Remove(0, base64String.IndexOf(',') + 1);
                        if (!string.IsNullOrEmpty(base64String))
                        {
                            fileSource = Convert.FromBase64String(fileData);
                            // byteDataCheck = objServiceHandler.CheckFileFormat(imageSource);
                            if (byteDataCheck == "data:image/jpeg" || byteDataCheck == "data:image/png" || byteDataCheck == "data:image/gif")
                            {
                                returnFilePath = objServiceHandler.SaveImageDirectory(Convert.ToString(imageName) + dto.ActivityImage, fileSource);
                            }
                            else if (byteDataCheck == "data:application/pdf")
                            {
                                var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images\TaskManagement\document", Convert.ToString(imageName) + dto.ActivityImage + ".pdf");
                                returnFilePath = @"images\TaskManagement\document\" + Convert.ToString(imageName) + dto.ActivityImage + ".pdf";
                                File.WriteAllBytes(filePath, fileSource);
                            }
                            else if (byteDataCheck == "data:application/octet-stream")
                            {
                                var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images\TaskManagement\document", Convert.ToString(imageName) + dto.ActivityImage + ".txt");
                                returnFilePath = @"images\TaskManagement\document\" + Convert.ToString(imageName) + dto.ActivityImage + ".txt";
                                File.WriteAllBytes(filePath, fileSource);
                            }
                            else if (byteDataCheck == "data:application/vnd.openxmlformats-officedocument.wordprocessingml.document")
                            {
                                var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images\TaskManagement\document", Convert.ToString(imageName) + dto.ActivityImage + ".docx");
                                returnFilePath = @"images\TaskManagement\document\" + Convert.ToString(imageName) + dto.ActivityImage + ".docx";
                                File.WriteAllBytes(filePath, fileSource);
                            }
                            else if (byteDataCheck == "data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" || byteDataCheck == "data:application/vnd.ms-excel")
                            {
                                var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images\TaskManagement\document", Convert.ToString(imageName) + dto.ActivityImage + ".xls");
                                returnFilePath = @"images\TaskManagement\document\" + Convert.ToString(imageName) + dto.ActivityImage + ".xls";
                                File.WriteAllBytes(filePath, fileSource);
                            }
                        }
                        returnFilePathList.Add(returnFilePath);

                    }
                }

                taskTaskActivityRepository = new TaskActivityRepository();
                var taskActivityDetails = await taskTaskActivityRepository.CreateTaskActivity(
                                                                                                decryptRemarks,
                                                                                                decryptEmpId,
                                                                                                requestDetaildto.UserId,
                                                                                                decryptTaskAssigneId,
                                                                                                returnFilePathList,
                                                                                                dto.StampAdmin
                                                                                                );
                if (taskActivityDetails != "")
                {
                    strSysText = requestDetaildto.Username + " has complete a task";
                }

                return "Task completion successful";
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

        public async Task<string> DeleteTaskActivityById(string taskActivityId, RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            try
            {
                _commonRepository = new CommonRepository();
                string decryptTaskActivityId = await _commonRepository.Decrypt(taskActivityId, requestDetaildto.SecurityKey);

                taskTaskActivityRepository = new TaskActivityRepository();

                string taskActivityDetails = await taskTaskActivityRepository.IsDeleteTaskActivityById(decryptTaskActivityId);

                strSysText = $"Get status list by { requestDetaildto.Username }";
                return taskActivityDetails;
            }
            catch (Exception e)
            {
                strSysText = e.Message.ToString();
                throw new Exception(strSysText);
            }
            finally
            {
                _commonRepository = new CommonRepository();
                await _commonRepository.SystemAudit(requestDetaildto, strSysText);
            }
        }

        public async Task<IEnumerable<TaskActivityDto>> GetTaskActivityById(string taskActivityId, RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            try
            {
                _commonRepository = new CommonRepository();
                string decrypTaskActivityId = await _commonRepository.Decrypt(taskActivityId, requestDetaildto.SecurityKey);

                taskTaskActivityRepository = new TaskActivityRepository();
                var taskActivityDetails = await taskTaskActivityRepository.GeTaskActivityListById(decrypTaskActivityId);
                var taskActivityDetailsMap = _mapper.Map<IEnumerable<TaskActivityDto>>(taskActivityDetails);

                strSysText = requestDetaildto.Username + " has successfully retrive a Task Activity " + decrypTaskActivityId;
                return taskActivityDetailsMap;
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

        public async Task<IEnumerable<TaskActivityDto>> TaskActivityByProjectModule(TaskActivityProjectModule ProjectModule, RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            try
            {
                _commonRepository = new CommonRepository();
                string decrypProjectId = await _commonRepository.Decrypt(ProjectModule.ProjectId, requestDetaildto.SecurityKey);
                string decrypModuleId = await _commonRepository.Decrypt(ProjectModule.ModuleId, requestDetaildto.SecurityKey);

                taskTaskActivityRepository = new TaskActivityRepository();
                var taskActivityDetails = await taskTaskActivityRepository.TaskActivityByProjectModule(decrypProjectId, decrypModuleId);
                var taskActivityDetailsMap = _mapper.Map<IEnumerable<TaskActivityDto>>(taskActivityDetails);

                strSysText = requestDetaildto.Username + " has successfully retrive a Task Activity ";
                return taskActivityDetailsMap;
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

        public async Task<IEnumerable<TaskActivityDto>> GetTaskActivityList(RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            try
            {
                taskTaskActivityRepository = new TaskActivityRepository();
                var taskActivities = await taskTaskActivityRepository.GetTaskActivityList(requestDetaildto.UserId);
                var taskActivitiesMap = _mapper.Map<IEnumerable<TaskActivityDto>>(taskActivities);

                strSysText = requestDetaildto.Username + " has successfully fetched Task Activity List";
                return taskActivitiesMap;
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

        public async Task<string> UpdateTaskActivityByID(TaskActivityDto dto, RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            string returnFilePath = "";
            string byteDataCheck = "";
            byte[] fileSource = null;
            string startTime = "";
            string endTime = "";
            int imageName = 0;
            try
            {
                List<string> returnFilePathList = new List<string>();

                _commonRepository = new CommonRepository();
                string decrypttaskActivityId = await _commonRepository.Decrypt(dto.ActivityId, requestDetaildto.SecurityKey);
                startTime = await _commonRepository.Decrypt(dto.StartTime, requestDetaildto.SecurityKey);
                endTime = await _commonRepository.Decrypt(dto.EndTime, requestDetaildto.SecurityKey);
                string decryptRemarks = await _commonRepository.Decrypt(dto.Remarks, requestDetaildto.SecurityKey);
                string decryptTaskStatusListId = await _commonRepository.Decrypt(dto.TaskStatusListId, requestDetaildto.SecurityKey);
                string decryptTaskAssigneId = await _commonRepository.Decrypt(dto.TaskAssigneeId, requestDetaildto.SecurityKey);
                string decryptEmpId = await _commonRepository.Decrypt(dto.EmpId, requestDetaildto.SecurityKey);
                string activityDate = await _commonRepository.Decrypt(dto.TaskactivityDate, requestDetaildto.SecurityKey);
                Regex reg = new Regex("[\"]");
                DateTime decryptStartTime = Convert.ToDateTime(reg.Replace(startTime, ""));
                DateTime decryptEndTime = Convert.ToDateTime(reg.Replace(endTime, ""));
                DateTime decryptActivityDate = Convert.ToDateTime(reg.Replace(activityDate, ""));
                if (dto.ActivityImageSource != null)
                {
                    foreach (var base64String in dto.ActivityImageSource)
                    {
                        imageName++;
                        byteDataCheck = base64String.Substring(0, base64String.IndexOf(';'));
                        var fileData = base64String.Remove(0, base64String.IndexOf(',') + 1);
                        if (!string.IsNullOrEmpty(base64String))
                        {
                            fileSource = Convert.FromBase64String(fileData);
                            // byteDataCheck = objServiceHandler.CheckFileFormat(imageSource);
                            if (byteDataCheck == "data:image/jpeg" || byteDataCheck == "data:image/png" || byteDataCheck == "data:image/gif")
                            {
                                returnFilePath = objServiceHandler.SaveImageDirectory(Convert.ToString(imageName) + dto.ActivityImage, fileSource);
                            }
                            else if (byteDataCheck == "data:application/pdf")
                            {
                                var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images\TaskManagement\document", Convert.ToString(imageName) + dto.ActivityImage + ".pdf");
                                returnFilePath = @"images\TaskManagement\document\" + Convert.ToString(imageName) + dto.ActivityImage + ".pdf";
                                File.WriteAllBytes(filePath, fileSource);
                            }
                            else if (byteDataCheck == "data:application/octet-stream")
                            {
                                var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images\TaskManagement\document", Convert.ToString(imageName) + dto.ActivityImage + ".txt");
                                returnFilePath = @"images\TaskManagement\document\" + Convert.ToString(imageName) + dto.ActivityImage + ".txt";
                                File.WriteAllBytes(filePath, fileSource);
                            }
                            else if (byteDataCheck == "data:application/vnd.openxmlformats-officedocument.wordprocessingml.document")
                            {
                                var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images\TaskManagement\document", Convert.ToString(imageName) + dto.ActivityImage + ".docx");
                                returnFilePath = @"images\TaskManagement\document\" + Convert.ToString(imageName) + dto.ActivityImage + ".docx";
                                File.WriteAllBytes(filePath, fileSource);
                            }
                            else if (byteDataCheck == "data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" || byteDataCheck == "data:application/vnd.ms-excel")

                            {
                                var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images\TaskManagement\document", Convert.ToString(imageName) + dto.ActivityImage + ".xls");
                                returnFilePath = @"images\TaskManagement\document\" + Convert.ToString(imageName) + dto.ActivityImage + ".xls";
                                File.WriteAllBytes(filePath, fileSource);
                            }
                        }
                        returnFilePathList.Add(returnFilePath);

                    }
                }

                taskTaskActivityRepository = new TaskActivityRepository();
                var taskActivities = await taskTaskActivityRepository.UpdateTaskActivityListByID(decrypttaskActivityId, decryptStartTime, decryptEndTime, decryptRemarks, decryptTaskStatusListId, decryptEmpId, requestDetaildto.UserId, decryptTaskAssigneId, decryptActivityDate, returnFilePathList);
                var taskActivitiesMap = _mapper.Map<IEnumerable<TaskActivityDto>>(taskActivities);

                strSysText = requestDetaildto.Username + " has successfully updated a Task Activity " + decrypttaskActivityId;
                return "Task Activity is  successfully updated ";
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


        public async Task<IEnumerable<DashboardAssignDetailsDto>> GetTaskCompletebyAdmin(RequestDetailDto requestDetaildto)
        {
            string strSysText = ""; int inCountInterval = 0;
            int inCountIntervalMinute = 0;
            bool timeExceed = false;
            int replacedHour = 0; int replacesMinute = 0;
            try
            {
                List<DashboardAssignDetailsDto> taskCompleteByAdminDto = new List<DashboardAssignDetailsDto>();
                _taskNotificationRepository = new TaskNotificationRepoitory();
                _getEmployeeRepository = new EmployeeListRepository();
                _taskManagementHelper = new TaskHelperRepository();
                var employee = await _taskManagementHelper.GetEmployeeByUserId(requestDetaildto.UserId);
                string empId = TaskAppHelper.GetFieldValueById(employee, "EMP_ID");
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
                                        getDataOnTaskAssignCountClick.EndTime = itemDuration.END_TIME;
                                        itemDuration.END_TIME = DateTime.Now;
                                        listStartTime.Add(itemDuration.START_TIME);
                                        listEndTime.Add(itemDuration.END_TIME);
                                        var countInterval = itemDuration.END_TIME - itemDuration.START_TIME;
                                        inCountIntervalMinute = inCountIntervalMinute + countInterval.Minutes;

                                        inCountInterval = inCountInterval + countInterval.Hours;
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
                            if (listEndTime.Count() > 0 && getDataOnTaskAssignCountClick.EndTime.ToString() != "1/1/0001 12:00:00 AM")
                            {
                                getDataOnTaskAssignCountClick.EndTime = listEndTime.Max(date1 => date1);
                            }
                            else
                            {
                                getDataOnTaskAssignCountClick.EndTime = null;
                            }
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
                        if (getDataOnTaskAssignCountClick.TimeExceed)
                        {
                           taskCompleteByAdminDto.Add(getDataOnTaskAssignCountClick);
                        }
                    }
                }
              
                strSysText = requestDetaildto.Username + " has successfully fetched Task Activity List";
                return taskCompleteByAdminDto;
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
    }
}
