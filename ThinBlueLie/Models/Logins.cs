using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ThinBlue
{
    public class Logins
    {
        [Key]
        public int IdLogin { get; set; }
        public string IdUser { get; set; } //id of AspNetUser
        [Column(TypeName = "DATETIME")]
        public string Time { get; set; } //        
        public string UserAgent { get; set; }
    }
}
