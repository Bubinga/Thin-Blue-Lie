using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLibrary.UserModels
{
    [Table("aspnetuserroles")]
    public partial class Aspnetuserroles
    {
      
        [Column(TypeName = "varchar(255)")]
        public string UserId { get; set; }
      
        [Column(TypeName = "varchar(255)")]
        public string RoleId { get; set; }

        [ForeignKey(nameof(RoleId))]
        [InverseProperty(nameof(Aspnetroles.Aspnetuserroles))]
        public virtual Aspnetroles Role { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(Aspnetusers.Aspnetuserroles))]
        public virtual Aspnetusers User { get; set; }
    }
}
