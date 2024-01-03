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
using BizWebAPI.Areas.ERP.Common;
using System.Globalization;
using System.Linq;

namespace BizWebAPI.Areas.ERP.Manager.TaskManagement
{
    public class TaskAssignManager : ITaskAssign
    {
        private TaskAssignRepository taskTaskAssignRepository;
        private TaskNotificationRepoitory _taskNotificationRepository;

        private DashBoardRepository _dashBoardRepository;
        private CommonRepository _commonRepository;
        private readonly IMapper _mapper;

        public TaskAssignManager(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<string> CommonMessage()
        {
            _commonRepository = new CommonRepository();
            return await _commonRepository.CommonMessage();
        }

        public async Task<IEnumerable<TaskAssignDto>> GetTaskAssignWiseName(RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            try
            {
                _taskNotificationRepository = new TaskNotificationRepoitory();

                taskTaskAssignRepository = new TaskAssignRepository();
                var taskAssignList = await taskTaskAssignRepository.GetTaskAssignWiseName(requestDetaildto.UserId);
                var taskAssignListMap = _mapper.Map<IEnumerable<TaskAssignDto>>(taskAssignList);
                strSysText = requestDetaildto.Username + " has successfully fetched Task Assign List";
                return taskAssignListMap;
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

        public async Task<IEnumerable<TaskAssignDto>> GetTaskAssignList(RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            int inCountInterval = 0;
            int inCountIntervalMinute = 0;
            bool timeExceed = false;
            int replacedHour = 0; int replacesMinute = 0;
            try
            {
                _taskNotificationRepository = new TaskNotificationRepoitory();

                List<DateTime> listStartTime = new List<DateTime>();
                List<DateTime> listEndTime = new List<DateTime>();
                taskTaskAssignRepository = new TaskAssignRepository();
                var taskAssignList = await taskTaskAssignRepository.GetTaskAssignList(requestDetaildto.UserId);
                foreach (var item in taskAssignList)
                {
                    _dashBoardRepository = new DashBoardRepository();
                    var duration = await _dashBoardRepository.GetAssignIdByStartEndDuration(item.TASK_ASSIGNEE_ID,"");
                    if (duration != null)
                    {
                        foreach (var itemDuration in duration)
                        {
                            if (itemDuration.START_TIME.ToString() == "1/1/0001 12:00:00 AM" || itemDuration.END_TIME.ToString() == "1/1/0001 12:00:00 AM")
                            { }
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
                      
                        DateTime estimateTimeDate;
                        DateTime extendTimespan;

                        if (DateTime.TryParse(item.ESTIMATED_TIME, out estimateTimeDate))
                        {
                            var dataTaskEstimateTime = await _taskNotificationRepository.TaskListIdEstimateTime("","", item.TASK_ASSIGNEE_ID);

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
                            //check if duration surpass estim time + extendtime 
                            if (replacedHour == inCountInterval && replacesMinute < inCountIntervalMinute )
                            {
                                    timeExceed = true;
                                    item.TIME_EXCEED = timeExceed;
                            }
                            else
                            {
                                if (replacedHour < inCountInterval)
                                {
                                    timeExceed = true;
                                    item.TIME_EXCEED = timeExceed;
                                }
                            }
                        }
                        else
                        {
                            if (Convert.ToInt32(item.ESTIMATED_TIME) < inCountInterval)
                            {
                                timeExceed = true;
                                item.TIME_EXCEED = timeExceed;
                            }
                        }
                        inCountInterval = 0;
                        inCountIntervalMinute = 0;
                        replacedHour = 0;
                        replacesMinute = 0;
                    }
                }
                var taskAssignListMap = _mapper.Map<IEnumerable<TaskAssignDto>>(taskAssignList);
                strSysText = requestDetaildto.Username + " has successfully fetched Task Assign List";
                return taskAssignListMap;
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

        public async Task<string> CreateTaskAssign(TaskAssignDto dto, RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            try
            {
                _commonRepository = new CommonRepository();
                string decryptTaskId = await _commonRepository.Decrypt(dto.TaskListId, requestDetaildto.SecurityKey);
                string decryptPriority = await _commonRepository.Decrypt(dto.Priority, requestDetaildto.SecurityKey);
                string decryptEmployeeId = await _commonRepository.Decrypt(dto.EmployeeId, requestDetaildto.SecurityKey);
                string assignDate = await _commonRepository.Decrypt(dto.AssignDate, requestDetaildto.SecurityKey);
                Regex reg = new Regex("[\"]");
                DateTime decryptAssignDate = Convert.ToDateTime(reg.Replace(assignDate, ""));

                taskTaskAssignRepository = new TaskAssignRepository();
                var taskAssignDetails = await taskTaskAssignRepository.CreateTaskAssign(decryptTaskId, decryptPriority, decryptAssignDate, requestDetaildto.UserId, decryptEmployeeId);

                strSysText = requestDetaildto.Username + " has successfully Assign a Task";
                return strSysText;
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

        public async Task<IEnumerable<TaskAssignDto>> GetTaskAssignById(string taskAssignId, RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            try
            {
                _commonRepository = new CommonRepository();
                string decrypTaskAssignId = await _commonRepository.Decrypt(taskAssignId, requestDetaildto.SecurityKey);

                taskTaskAssignRepository = new TaskAssignRepository();
                var taskAssignDetails = await taskTaskAssignRepository.GeTaskAssignListById(decrypTaskAssignId);
                var taskAssignDetailsMap = _mapper.Map<IEnumerable<TaskAssignDto>>(taskAssignDetails);

                strSysText = requestDetaildto.Username + " has successfully retrive a Task " + taskAssignId;
                return taskAssignDetailsMap;
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

        public async Task<IEnumerable<TaskAssignDto>> UpdateTaskAssignByID(TaskAssignDto taskAssignDto, RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            try
            {
                _commonRepository = new CommonRepository();
                string decrypTaskAssignId = await _commonRepository.Decrypt(taskAssignDto.TaskAssignId, requestDetaildto.SecurityKey);
                string decryptTaskId = await _commonRepository.Decrypt(taskAssignDto.TaskListId, requestDetaildto.SecurityKey);
                string decryptPriority = await _commonRepository.Decrypt(taskAssignDto.Priority, requestDetaildto.SecurityKey);
                string decryptEmployeeId = await _commonRepository.Decrypt(taskAssignDto.EmployeeId, requestDetaildto.SecurityKey);

                taskTaskAssignRepository = new TaskAssignRepository();
                var taskAssignDetails = await taskTaskAssignRepository.UpdateTaskAssignListByID(decrypTaskAssignId, decryptTaskId, decryptPriority, requestDetaildto.UserId, decryptEmployeeId);
                var taskAssignDetailsMap = _mapper.Map<IEnumerable<TaskAssignDto>>(taskAssignDetails);

                strSysText = requestDetaildto.Username + " has successfully updated a Task Assign " + decrypTaskAssignId;
                return taskAssignDetailsMap;
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

        public async Task<string> TaskSatarPause(TaskTempActivitryDto tempActivity, RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            try
            {
                _commonRepository = new CommonRepository();
                string decryptTaskAssignId = await _commonRepository.Decrypt(tempActivity.TaskAssignId, requestDetaildto.SecurityKey);
                string decryptTaskActivityType = await _commonRepository.Decrypt(tempActivity.TempActivityType, requestDetaildto.SecurityKey);

                var empId = await _commonRepository.Decrypt(tempActivity.EmpId, requestDetaildto.SecurityKey);
                taskTaskAssignRepository = new TaskAssignRepository();
                string taskAssignDetails = await taskTaskAssignRepository.TaskSatarPause(decryptTaskAssignId,
                                                                                        decryptTaskActivityType,
                                                                                        requestDetaildto.UserId,
                                                                                        empId
                                                                                        );

                strSysText = "Temp task Start Pause activity " + requestDetaildto.Username;
                return taskAssignDetails;
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

        public async Task<string> AssignTimeExtend(AssignTimeExtendDto assignTimeExtend, RequestDetailDto requestDetaildto)
        {
            string strSysText = ""; string decryptExtendTime = "";string replacedTime = "";
            try
            {
                _commonRepository = new CommonRepository();
                string decryptEmployeeId = await _commonRepository.Decrypt(assignTimeExtend.EmpId, requestDetaildto.SecurityKey);
                string decryptTaskAssignId = await _commonRepository.Decrypt(assignTimeExtend.TaskAssignId, requestDetaildto.SecurityKey);
                string decryptTimeExtendNote = await _commonRepository.Decrypt(assignTimeExtend.TimeExtendNote, requestDetaildto.SecurityKey);
                 decryptExtendTime = await _commonRepository.Decrypt(assignTimeExtend.ExtendTime, requestDetaildto.SecurityKey);

                _taskNotificationRepository = new TaskNotificationRepoitory();
                var dataTaskEstimateTime = await _taskNotificationRepository.TaskListIdEstimateTime("", decryptTaskAssignId,"");
                foreach (var itemdataTaskEstim in dataTaskEstimateTime)
                {
                    DateTime dDate;

                    if (DateTime.TryParse(itemdataTaskEstim.EXTEND_TIME, out dDate))
                    {
                        var x = Convert.ToDateTime(decryptExtendTime);
                        var replacedHour = dDate.Hour + x.Hour;
                        var replacedMinute = dDate.Minute + x.Minute;
                        replacedHour = replacedHour + (replacedMinute / 60);
                        replacedMinute = replacedMinute % 60;
                        replacedTime = replacedHour.ToString() + ":" + replacedMinute.ToString();
                    }
                    else
                    {
                        if(string.IsNullOrEmpty(itemdataTaskEstim.ESTIMATED_TIME))
                        replacedTime = (Convert.ToInt32(itemdataTaskEstim.EXTEND_TIME) + Convert.ToDateTime(decryptExtendTime).Hour).ToString() + ":" + Convert.ToDateTime(decryptExtendTime).Minute;
                    }

                }
                taskTaskAssignRepository = new TaskAssignRepository();
                string taskAssignDetails = await taskTaskAssignRepository.AssignTimeExtend(decryptEmployeeId,decryptTaskAssignId,
                                                                                            decryptTimeExtendNote, replacedTime
                                                                                            );

                strSysText = "Temp task Start Pause activity " + requestDetaildto.Username;
                return taskAssignDetails;
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

        public async Task<string> DeleteTaskAssignById(string taskAssignId, RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            try
            {
                _commonRepository = new CommonRepository();
                string decryptTaskAssignId = await _commonRepository.Decrypt(taskAssignId, requestDetaildto.SecurityKey);

                taskTaskAssignRepository = new TaskAssignRepository();
                string taskAssignDetails = await taskTaskAssignRepository.IsDeleteTaskAssignById(decryptTaskAssignId);

                strSysText = "Delete assign task by " + requestDetaildto.Username + ", Task id " + taskAssignId;
                return taskAssignDetails;
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
