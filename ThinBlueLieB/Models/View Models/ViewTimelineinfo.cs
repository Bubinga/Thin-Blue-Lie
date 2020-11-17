using DataAccessLibrary.Enums;
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
        [Required(ErrorMessage = "Please enter a Title")]
        [MaxLength(70, ErrorMessage = "Please enter less than 70 characters")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Please enter a Date")]
        [MaxLength(10, ErrorMessage = "Please enter a date in the format 'yyyy-MM-dd'")]
        public string Date { get; set; }
        public TimelineinfoEnums.StateEnum? State { get; set; }
        [Required(ErrorMessage ="Please enter a City")]
        [MaxLength(30, ErrorMessage ="City name must be shorter than 30 characters")]
        public string City { get; set; }     
        [Required]
        [MinLength(75, ErrorMessage = "Please type at least 75 characters")]
        public string Context { get; set; }
        public byte Locked { get; set; }
        public int? SubmittedBy { get; set; }
        public byte Verified { get; set; }
    }
}
