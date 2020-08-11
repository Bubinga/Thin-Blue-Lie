using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLibrary.UserModels
{
    [Table("aspnetuserclaims")]
    public partial class Aspnetuserclaims
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "varchar(255)")]
        public string UserId { get; set; }
        [Column(TypeName = "longtext")]
        public string ClaimType { get; set; }
        [Column(TypeName = "longtext")]
        public string ClaimValue { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(Aspnetusers.Aspnetuserclaims))]
        public virtual Aspnetusers User { get; set; }
    }
}
