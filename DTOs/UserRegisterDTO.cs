using System.ComponentModel.DataAnnotations;

namespace Company.DTOs
{
    public class UserRegisterDTO
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        public int PermissionID { get; set; }
    }
}
