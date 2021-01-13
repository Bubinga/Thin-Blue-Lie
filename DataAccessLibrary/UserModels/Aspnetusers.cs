using DataAccessLibrary.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLibrary.UserModels
{
    [Table("users")]
    public partial class Aspnetusers
    {
        public Aspnetusers()
        {
            EditMedia = new HashSet<EditMedia>();
            Edits = new HashSet<Edits>();
            Flagged = new HashSet<Flags>();
            Log = new HashSet<Log>();
            Media = new HashSet<Media>();
            //Timelineinfo = new HashSet<TimelineinfoFull>();
            Aspnetuserclaims = new HashSet<Aspnetuserclaims>();
            Aspnetuserlogins = new HashSet<Aspnetuserlogins>();
            Aspnetuserroles = new HashSet<Aspnetuserroles>();
            Aspnetusertokens = new HashSet<Aspnetusertokens>();
        }

        [Key]
        [Column(TypeName = "varchar(255)")]
        public string Id { get; set; }
        [Column(TypeName = "varchar(256)")]
        public string UserName { get; set; }
        [Column(TypeName = "varchar(256)")]
        public string NormalizedUserName { get; set; }
        [Column(TypeName = "varchar(256)")]
        public string Email { get; set; }
        [Column(TypeName = "varchar(256)")]
        public string NormalizedEmail { get; set; }
        [Column(TypeName = "bit(1)")]
        public ulong EmailConfirmed { get; set; }
        [Column(TypeName = "longtext")]
        public string PasswordHash { get; set; }
        [Column(TypeName = "longtext")]
        public string SecurityStamp { get; set; }
        [Column(TypeName = "longtext")]
        public string ConcurrencyStamp { get; set; }
        [Column(TypeName = "longtext")]
        public string PhoneNumber { get; set; }
        [Column(TypeName = "bit(1)")]
        public ulong PhoneNumberConfirmed { get; set; }
        [Column(TypeName = "bit(1)")]
        public ulong TwoFactorEnabled { get; set; }
        public DateTime? LockoutEnd { get; set; }
        [Column(TypeName = "bit(1)")]
        public ulong LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<Aspnetuserclaims> Aspnetuserclaims { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<Aspnetuserlogins> Aspnetuserlogins { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<Aspnetuserroles> Aspnetuserroles { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<Aspnetusertokens> Aspnetusertokens { get; set; }
        [InverseProperty("SubmittedByNavigation")]
        public virtual ICollection<EditMedia> EditMedia { get; set; }
        [InverseProperty("SubmittedByNavigation")]
        public virtual ICollection<Edits> Edits { get; set; }
        [InverseProperty("UserIdNavigation")]
        public virtual ICollection<Flags> Flagged { get; set; }
        [InverseProperty("UserIdNavigation")]
        public virtual ICollection<Log> Log { get; set; }
        [InverseProperty("SubmittedByNavigation")]
        public virtual ICollection<Media> Media { get; set; }
        //[InverseProperty("SubmittedByNavigation")]
        //public virtual ICollection<TimelineinfoFull> Timelineinfo { get; set; }
    }
}
