using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLibrary.UserModels
{
    [Table("roles")]
    public partial class Aspnetroles
    {
        public Aspnetroles()
        {
            Aspnetroleclaims = new HashSet<Aspnetroleclaims>();
            Aspnetuserroles = new HashSet<Aspnetuserroles>();
        }

        [Key]
        [Column(TypeName = "varchar(255)")]
        public string Id { get; set; }
        [Column(TypeName = "varchar(256)")]
        public string Name { get; set; }
        [Column(TypeName = "varchar(256)")]
        public string NormalizedName { get; set; }
        [Column(TypeName = "longtext")]
        public string ConcurrencyStamp { get; set; }

        [InverseProperty("Role")]
        public virtual ICollection<Aspnetroleclaims> Aspnetroleclaims { get; set; }
        [InverseProperty("Role")]
        public virtual ICollection<Aspnetuserroles> Aspnetuserroles { get; set; }
    }
}
