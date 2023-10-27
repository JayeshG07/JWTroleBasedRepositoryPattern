using System.ComponentModel.DataAnnotations;

namespace JWTroleBased.Models
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }
        public string Name { get; set; }
        //public ICollection<User> Users { get; set; }
    }
}
