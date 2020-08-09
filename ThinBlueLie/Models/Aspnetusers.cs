using System;
using System.Collections.Generic;

namespace ThinBlue
{
    public partial class Aspnetusers
    {
        public Aspnetusers()
        {
            EditMedia = new HashSet<EditMedia>();
            Edits = new HashSet<Edits>();
            Flagged = new HashSet<Flagged>();
            Log = new HashSet<Log>();
            Media = new HashSet<Media>();
            Timelineinfo = new HashSet<Timelineinfo>();
            Aspnetuserclaims = new HashSet<Aspnetuserclaims>();
            Aspnetuserlogins = new HashSet<Aspnetuserlogins>();
            Aspnetuserroles = new HashSet<Aspnetuserroles>();
            Aspnetusertokens = new HashSet<Aspnetusertokens>();
        }

        public string Id { get; set; }
        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public short EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string PhoneNumber { get; set; }
        public short PhoneNumberConfirmed { get; set; }
        public short TwoFactorEnabled { get; set; }
        public DateTime? LockoutEnd { get; set; }
        public short LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }

        public virtual ICollection<EditMedia> EditMedia { get; set; }
        public virtual ICollection<Edits> Edits { get; set; }
        public virtual ICollection<Flagged> Flagged { get; set; }
        public virtual ICollection<Log> Log { get; set; }
        public virtual ICollection<Media> Media { get; set; }
        public virtual ICollection<Timelineinfo> Timelineinfo { get; set; }
        public virtual ICollection<Aspnetuserclaims> Aspnetuserclaims { get; set; }
        public virtual ICollection<Aspnetuserlogins> Aspnetuserlogins { get; set; }
        public virtual ICollection<Aspnetuserroles> Aspnetuserroles { get; set; }
        public virtual ICollection<Aspnetusertokens> Aspnetusertokens { get; set; }
    }
}
