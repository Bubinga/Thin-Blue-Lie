using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLibrary.DataModels
{
    [Table("officers")]
    public partial class Officers
    {
        public Officers()
        {
            TimelineinfoOfficer = new HashSet<TimelineinfoOfficer>();
        }

        [Key]
        public int IdOfficer { get; set; }
        [Column(TypeName = "varchar(60)")]
        public string Name { get; set; }
        public byte Race { get; set; }
        public byte Sex { get; set; }

        [InverseProperty("IdOfficerNavigation")]
        public virtual ICollection<TimelineinfoOfficer> TimelineinfoOfficer { get; set; }
    }
}
