using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLibrary.UserModels
{
    [Table("aspnetroleclaims")]
    public partial class Aspnetroleclaims
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "varchar(255)")]
        public string RoleId { get; set; }
        [Column(TypeName = "longtext")]
        public string ClaimType { get; set; }
        [Column(TypeName = "longtext")]
        public string ClaimValue { get; set; }

        [ForeignKey(nameof(RoleId))]
        [InverseProperty(nameof(Aspnetroles.Aspnetroleclaims))]
        public virtual Aspnetroles Role { get; set; }
    }
}
