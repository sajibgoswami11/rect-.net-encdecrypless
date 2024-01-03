using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.Dtos.TaskManagement
{
    public class TaskNotificationDto
    {
        public string NotificationId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string SeenStatus { get; set; }
        public string SeenBy { get; set; }
        public string CreateBy { get; set; }
        public string Link { get; set; }
        public bool IsSelected { get; set; }
        public string ExtendTime { get; set; }
    }
}
