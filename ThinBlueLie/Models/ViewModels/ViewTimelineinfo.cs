using DataAccessLibrary.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static DataAccessLibrary.Enums.TimelineinfoEnums;

namespace ThinBlueLie.Models
{
    public class ViewTimelineinfo
    {
        public int IdTimelineinfo { get; set; }
        [Required(ErrorMessage = "Please enter a Title")]
        [MaxLength(70, ErrorMessage = "Please enter less than 70 characters")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Please enter a Date")]
        public DateTime Date { get; set; }
        [Required(ErrorMessage = "Please select a State")]
        public TimelineinfoEnums.StateEnum? State { get; set; }
        [Required(ErrorMessage ="Please enter a City")]
        [MaxLength(30, ErrorMessage ="City name must be shorter than 30 characters")]
        public string City { get; set; }     
        [Required]
        [MinLength(75, ErrorMessage = "Please type at least 75 characters")]
        public string Context { get; set; }
        public SupDataEnum SupData { get; set; }
        public byte Locked { get; set; }
        public int? SubmittedBy { get; set; }
    }
}
