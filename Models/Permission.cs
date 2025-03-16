using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Company.Models
{
    [Table("Permission")]
    public class Permission
    {
        public int PermissionID { get; private set; }
        public string PermissionName { get; set; }
    }
}
