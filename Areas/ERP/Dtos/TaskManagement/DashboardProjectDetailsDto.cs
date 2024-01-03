using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.Dtos.TaskManagement
{
    public class DashboardProjectDetailsDto
    {
        public string ProjectName { get; set; }
        public string Milestones { get; set; }
        public string CreateBy { get; set; }
        //public string TeamMember { get; set; }
        public int TotalTask { get; set; }
        public int CompleteTask { get; set; }
    }
}
