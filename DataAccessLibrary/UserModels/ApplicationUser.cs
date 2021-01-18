using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLibrary.UserModels
{
    public class ApplicationUser : IdentityUser<int>
    {
        public DateTime DateJoined { get; set; }
        public int Reputation { get; set; } = 1;
    }
}
