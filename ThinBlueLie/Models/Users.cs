using System;
using System.Collections.Generic;

namespace ThinBlue
{
    public partial class Users
    {
        public Users()
        {
            Userclaims = new HashSet<Userclaims>();
            Userlogins = new HashSet<Userlogins>();
            Userroles = new HashSet<Userroles>();
        }

        public int Id { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }        
        public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTime? LockoutEndDateUtc { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string Username { get; set; }

        public virtual ICollection<Userclaims> Userclaims { get; set; }
        public virtual ICollection<Userlogins> Userlogins { get; set; }
        public virtual ICollection<Userroles> Userroles { get; set; }
    }
}
