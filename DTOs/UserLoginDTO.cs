using System.ComponentModel.DataAnnotations;

namespace Company.DTOs
{
    public class UserLoginDTO
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
