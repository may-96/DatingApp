using System.ComponentModel.DataAnnotations;

namespace DemoApp.DTOs
{
    public class LoginDTO
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}