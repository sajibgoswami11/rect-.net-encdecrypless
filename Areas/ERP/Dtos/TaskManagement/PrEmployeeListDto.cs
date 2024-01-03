namespace BizWebAPI.Areas.ERP.Dtos.TaskManagement
{
    public class PrEmployeeListDto
    {
        public string EmpName { get; set; }
        public string EmpId { get; set; }
        public string EmpTitle { get; set; }
        public string EmpEmail { get; set; }
        public string EmpCode { get; set; }
        public string SysUserId { get; set; }
        public string CmpBranchId { get; set; }
        public string ContactNumber { get; set; }
        public string PresentAddress { get; set; }
        public string PermanentAddress { get; set; }
        public string ImagePath { get; set; }
        public string NidCard { get; set; }
        public string[] ImageSource { get; set; }
        public string DepartmentId { get; set; }
        public string DesignationId { get; set; }
        public string AccountNo { get; set; }
        public string JoiningDate { get; set; }
        public string BirthDate { get; set; }

    }
}
