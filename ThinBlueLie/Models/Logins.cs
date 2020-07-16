using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ThinBlue
{
    public class Logins
    {
        [Key]
        public int IdLogin { get; set; }
        public string Id { get; set; } //id of AspNetUser
        public string Time { get; set; }
        public string IpAddress { get; set; }
        public string UserAgent { get; set; }
    }
}
