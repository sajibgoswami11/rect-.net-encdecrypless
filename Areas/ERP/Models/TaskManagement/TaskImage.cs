using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.Models.TaskManagement
{
    public class TaskImage
    {
        public string IMAGE_ID { get; set; }
        public string IMAGE_PATH { get; set; }
        public string REFERENCE_ID { get; set; }
        public string ISDELETE { get; set; }
    }
}
