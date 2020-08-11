using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLibrary.UserModels
{
    [Table("aspnetusertokens")]
    public partial class Aspnetusertokens
    {
        [Key]
        [Column(TypeName = "varchar(255)")]
        public string UserId { get; set; }
        [Key]
        [Column(TypeName = "varchar(255)")]
        public string LoginProvider { get; set; }
        [Key]
        [Column(TypeName = "varchar(255)")]
        public string Name { get; set; }
        [Column(TypeName = "longtext")]
        public string Value { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(Aspnetusers.Aspnetusertokens))]
        public virtual Aspnetusers User { get; set; }
    }
}
