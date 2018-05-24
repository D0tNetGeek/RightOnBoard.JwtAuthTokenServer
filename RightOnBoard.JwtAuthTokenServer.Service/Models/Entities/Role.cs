using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace RightOnBoard.JwtAuthTokenServer.Service.Models.Entities
{
    //public class Role : IdentityRole<string>
    //{
    //    public Role()
    //    {
    //        UserRoles = new HashSet<UserRole>();
    //    }        

    //    public string Id { get; set; }
    //    public string Name { get; set; }

    //    public virtual ICollection<UserRole> UserRoles { get; set; }
    //}

    public class Role : IdentityRole
    {
        public Role()
        {
            UserRoles = new HashSet<UserRole>();
        }

        public string Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
