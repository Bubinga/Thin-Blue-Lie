using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ThinBlueLieB.Models
{
    public class ViewTimelineinfo
    {
        public int IdTimelineinfo { get; set; }
        [Required]
        [MaxLength(10)]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public byte State { get; set; }
        [Required]
        [MaxLength(30, ErrorMessage ="City name must be shorter than 30 characters")]
        public string City { get; set; }     
        [Required]
        [MinLength(75, ErrorMessage = "Please type at least 75 characters")]
        public string Context { get; set; }
        public byte Locked { get; set; }
        public string? SubmittedBy { get; set; }
        public byte Verified { get; set; }
    }
}
