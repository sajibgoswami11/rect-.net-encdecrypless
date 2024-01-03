using System.ComponentModel.DataAnnotations;

namespace BizWebAPI.Areas.xEmptyArea.Dtos
{
    public class xEmptyAuthDto
    {
        public string Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
