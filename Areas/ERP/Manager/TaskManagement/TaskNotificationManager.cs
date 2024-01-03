using AutoMapper;
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
    public class TaskNotificationManager : INotification
    {
        private CommonRepository _commonRepository;
        private TaskNotificationRepoitory _taskNotificationRepository;
        private readonly IMapper _mapper;

        public TaskNotificationManager(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<string> CommonMessage()
        {
            _commonRepository = new CommonRepository();
            return await _commonRepository.CommonMessage();
        }
        public async Task<IEnumerable<TaskNotificationDto>> GetNotification(RequestDetailDto requestDetaildto)
        {
            string strSysText = "";
            try
            {
                _taskNotificationRepository = new TaskNotificationRepoitory();
                var notificationDetails = await _taskNotificationRepository.GetNotification();
                var notificationDetailsMap = _mapper.Map<IEnumerable<TaskNotificationDto>>(notificationDetails);
                return notificationDetailsMap.ToList();
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

        public async Task<string> PostNotifications(IEnumerable<TaskNotificationDto> taskNotificationDto, RequestDetailDto requestDetailDto)
        {
            string strSysText = "";
            string decryptNotificationId = ""; string replacedTime = "";
            try
            {
                foreach (var item in taskNotificationDto)
                { 
                    _commonRepository = new CommonRepository();
                    decryptNotificationId = await _commonRepository.Decrypt(item.NotificationId, requestDetailDto.SecurityKey);
                    string decrypExtendTime = await _commonRepository.Decrypt(item.ExtendTime, requestDetailDto.SecurityKey);
                    _taskNotificationRepository = new TaskNotificationRepoitory();

                    var dataTaskEstimateTime = await _taskNotificationRepository.TaskListIdEstimateTime(decryptNotificationId, "","");
                    foreach (var itemdataTaskEstim in dataTaskEstimateTime)
                    {
                        DateTime dDate;

                        if (DateTime.TryParse(itemdataTaskEstim.EXTEND_TIME, out dDate))
                        {
                            replacedTime = itemdataTaskEstim.EXTEND_TIME.ToString() ;
                        }
                        var notify =await _taskNotificationRepository.PostNotifications(decryptNotificationId, replacedTime);

                        //var dataforExtend = await _taskNotificationRepository.AddExtendedTimetoEstimatedOne(replacedTime, decryptNotificationId);
                    }
                }

                strSysText = requestDetailDto.Username + " has Successfully posted notifications ";
                return " Successfully posted notifications";
            }
            catch (Exception ex)
            {
                strSysText = ex.Message.ToString();
                throw new Exception(strSysText);
            }
            finally
            {
                _commonRepository = new CommonRepository();
                await _commonRepository.SystemAudit(requestDetailDto, strSysText);
            }
        }
      
        public async Task<IEnumerable<TaskAssignDto>> TaskIdByAssignIdwise(TaskNotificationDto taskNotificationDto, RequestDetailDto requestDetailDto)
        {
            string strSysText = "";
            try
            {
                _commonRepository = new CommonRepository();
                string decryptAssignId = await _commonRepository.Decrypt(taskNotificationDto.Link, requestDetailDto.SecurityKey);
                _taskNotificationRepository = new TaskNotificationRepoitory();    
                var taskIdnotification = await _taskNotificationRepository.TaskIdByAssignIdwise(decryptAssignId);
                var taskIdnotificationMap = _mapper.Map<IEnumerable<TaskAssignDto>>(taskIdnotification);

                return taskIdnotificationMap;
            }
            catch (Exception ex)
            {
                strSysText = ex.Message.ToString();
                throw new Exception(strSysText);
            }
            finally
            {
                _commonRepository = new CommonRepository();
                await _commonRepository.SystemAudit(requestDetailDto, strSysText);
            }
        }
     }
}
