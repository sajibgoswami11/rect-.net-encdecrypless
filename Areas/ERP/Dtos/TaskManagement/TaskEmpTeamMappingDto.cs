using System.ComponentModel.DataAnnotations.Schema;

namespace BizWebAPI.Areas.ERP.Dtos.TaskManagement
{
    public class TaskEmpTeamMappingDto
    {
        public string TeamId { get; set; }
        public string EmpName { get; set; }
        public string EmpId { get; set; }
        public string EmpImage { get; set; }
    }
}
