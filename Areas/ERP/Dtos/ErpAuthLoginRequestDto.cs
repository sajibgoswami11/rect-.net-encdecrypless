using System.ComponentModel.DataAnnotations;

namespace BizWebAPI.Areas.ERP.Dtos
{
    public class ErpAuthLoginRequestDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
