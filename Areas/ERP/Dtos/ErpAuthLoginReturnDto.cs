using System.ComponentModel.DataAnnotations;

namespace BizWebAPI.Areas.ERP.Dtos
{
    public class ErpAuthLoginReturnDto
    {
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Email { get; set; }
    }
}
