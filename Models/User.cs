using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Company.Models
{
    [Table("User")]
    public class User
    {
        public int Id { get; private set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }
        public int PermissionID { get; set; }
    }
}
