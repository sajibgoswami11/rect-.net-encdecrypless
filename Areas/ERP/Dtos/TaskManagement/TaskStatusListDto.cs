namespace BizWebAPI.Areas.ERP.Dtos.TaskManagement
{
    public class TaskStatusListDto
    {
        public string TaskStatusListId { get; set; }
        public string StatusName { get; set; }
        public string ProgressStatus { get; set; }
        public string StatusCategoryId { get; set; }
    }
}
