using BizWebAPI.Areas.ERP.Dtos;
using BizWebAPI.Areas.ERP.Dtos.TaskManagement;
using BizWebAPI.Areas.ERP.Models.TaskManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.Interfaces.TaskManagement
{
    public interface INotification
    {
        Task<string> CommonMessage();
        Task<IEnumerable<TaskNotificationDto>> GetNotification(RequestDetailDto userDetails);
        Task<string> PostNotifications(IEnumerable<TaskNotificationDto> taskNotificationDto, RequestDetailDto requestDetailDto);
        Task<IEnumerable<TaskAssignDto>> TaskIdByAssignIdwise(TaskNotificationDto taskNotificationDto, RequestDetailDto requestDetailDto);
    }
}
