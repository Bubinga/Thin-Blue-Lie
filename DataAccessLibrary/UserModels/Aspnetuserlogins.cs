using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLibrary.UserModels
{
    [Table("aspnetuserlogins")]
    public partial class Aspnetuserlogins
    {
        
        [Column(TypeName = "varchar(255)")]
        public string LoginProvider { get; set; }
        
        [Column(TypeName = "varchar(255)")]
        public string ProviderKey { get; set; }
        [Column(TypeName = "longtext")]
        public string ProviderDisplayName { get; set; }
        [Required]
        [Column(TypeName = "varchar(255)")]
        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(Aspnetusers.Aspnetuserlogins))]
        public virtual Aspnetusers User { get; set; }
    }
}
