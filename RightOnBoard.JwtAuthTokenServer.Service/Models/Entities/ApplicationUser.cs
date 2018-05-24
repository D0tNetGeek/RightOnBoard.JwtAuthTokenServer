using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace RightOnBoard.JwtAuthTokenServer.Service.Models.Entities
{
    //public class ApplicationUser : IdentityUser<string>
    //{
    //    public ApplicationUser()
    //    {
    //        UserRoles = new HashSet<UserRole>();
    //        UserTokens = new HashSet<UserToken>();
    //    }

    //    //Exteded Properties
    //    public string SerialNumber { get; set; }
    //    public string FirstName { get; set; }
    //    public string LastName { get; set; }
    //    public string Issuer { get; set; }
    //    public DateTimeOffset? LastLoggedIn { get; set; }
    //    public bool? IsActive { get; set; }

    //    public virtual ICollection<UserRole> UserRoles { get; set; }
    //    public virtual ICollection<UserToken> UserTokens { get; set; }
    //}

    public class ApplicationUser : IdentityUser<string>
    {
        public ApplicationUser()
        {
            UserRoles = new HashSet<UserRole>();
            UserTokens = new HashSet<UserToken>();
        }

        public string Id { get; set; }

        public string Username { get; set; }

       // public string Password { get; set; }

        //public string DisplayName { get; set; }

        public bool IsActive { get; set; }

        public DateTimeOffset? LastLoggedIn { get; set; }

        public string Issuer { get; set; }

        /// <summary>
        /// every time the user changes his Password,
        /// or an admin changes his Roles or stat/IsActive,
        /// create a new `SerialNumber` GUID and store it in the DB.
        /// </summary>
        public string SerialNumber { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }

        public virtual ICollection<UserToken> UserTokens { get; set; }
    }
}
