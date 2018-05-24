using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace RightOnBoard.JwtAuthTokenServer.Service.Models.Entities
{
    //public class UserRole : IdentityUserRole<string>
    //{
    //    public string UserId { get; set; }
    //    public string RoleId { get; set; }

    //    public virtual ApplicationUser User { get; set; }
    //    public virtual Role Role { get; set; }
    //}
    public class UserRole : IdentityUserRole<string>
    {
        [Key]
        public string Id { get; set; }
        public string UserId { get; set; }
        public string RoleId { get; set; }

        //[ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
        //[ForeignKey("RoleId")]
        public virtual Role Role { get; set; }
    }
}
