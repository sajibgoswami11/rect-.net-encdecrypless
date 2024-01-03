using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BizWebAPI.Areas.ERP.Dtos.TaskManagement
{
    public class DashBoardPercentageDto
    {
        public string ProjectName { get; set; }
        public string ProjectId { get; set; }
        public string TotalTask { get; set; }
        public string Complite { get; set; }
        public string RemainingTask { get; set; }

        #region taskdetails by project
        public string EmpName { get; set; }
        public string TaskDetails { get; set; }
        public string EstimateTime { get; set; }
        public string TaskImage { get; set; }
        public string AssigneeDate { get; set; }
        public string ActivityStatus { get; set; }
        public string ActivityId { get; set; }
        public string ActivityCreateDate { get; set; }
        public string ActivityUpdateDate { get; set; }
        public string TempActivityType { get; set; }
        public string TaskActivityduration { get; set; }
        public string TaskActivityDurationInMinutes { get; set; }
        #endregion

    }
}
