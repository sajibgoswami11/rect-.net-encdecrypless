using System.ComponentModel.DataAnnotations;

namespace BizWebAPI.Areas.ERP.Dtos
{
    public class ErpAuthLoginDto
    {
        public string UserId { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public string UserGroup { get; set; }
        public string EmpId { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Email { get; set; }
    }
}
