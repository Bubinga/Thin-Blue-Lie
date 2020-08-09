using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ThinBlue
{
    public partial class Officers
    {
        public Officers()
        {
            TimelineinfoOfficer = new HashSet<TimelineinfoOfficer>();
        }

        public int IdOfficer { get; set; }
        public string Name { get; set; }
        [Required(ErrorMessage = "The Officer's Race field is required")]
        public byte Race { get; set; }
        [Required(ErrorMessage = "The Officer's Sex field is required")]
        public byte Sex { get; set; }

        public virtual ICollection<TimelineinfoOfficer> TimelineinfoOfficer { get; set; }
    }
}
