using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThinBlueLieB.Models.Account_Models
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime DateJoined { get; set; }
        public int Reputation { get; set; }
    }
}
