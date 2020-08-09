using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;


namespace ThinBlue
{
    public partial class ThinbluelieContext : IdentityDbContext<IdentityUser>
    {
        public ThinbluelieContext()
        {
        }

        public ThinbluelieContext(DbContextOptions<ThinbluelieContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Aspnetroleclaims> Aspnetroleclaims { get; set; }
        public virtual DbSet<Aspnetroles> Aspnetroles { get; set; }
        public virtual DbSet<Aspnetuserclaims> Aspnetuserclaims { get; set; }
        public virtual DbSet<Aspnetuserlogins> Aspnetuserlogins { get; set; }
        public virtual DbSet<Aspnetuserroles> Aspnetuserroles { get; set; }
        public virtual DbSet<Aspnetusers> Aspnetusers { get; set; }
        public virtual DbSet<Aspnetusertokens> Aspnetusertokens { get; set; }
        public virtual DbSet<EditMedia> EditMedia { get; set; }
        public virtual DbSet<Edits> Edits { get; set; }
        public virtual DbSet<Flagged> Flagged { get; set; }
        public virtual DbSet<Log> Log { get; set; }
        public virtual DbSet<Media> Media { get; set; }
        public virtual DbSet<Officers> Officers { get; set; }
        public virtual DbSet<Subjects> Subjects { get; set; }
        public virtual DbSet<Timelineinfo> Timelineinfo { get; set; }
        public virtual DbSet<TimelineinfoOfficer> TimelineinfoOfficer { get; set; }
        public virtual DbSet<TimelineinfoSubject> TimelineinfoSubject { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
              //To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
              //  optionsBuilder.UseMySQL("server=localhost;port=3306;database=thin-blue-lie;user=root;password=Holland173$;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Aspnetroleclaims>(entity =>
            {
                entity.ToTable("aspnetroleclaims");

                entity.HasIndex(e => e.RoleId)
                    .HasName("IX_AspNetRoleClaims_RoleId");

                entity.Property(e => e.ClaimType).HasColumnType("longtext");

                entity.Property(e => e.ClaimValue).HasColumnType("longtext");

                entity.Property(e => e.RoleId)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Aspnetroleclaims)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_AspNetRoleClaims_AspNetRoles_RoleId");
            });

            modelBuilder.Entity<Aspnetroles>(entity =>
            {
                entity.ToTable("aspnetroles");

                entity.HasIndex(e => e.NormalizedName)
                    .HasName("RoleNameIndex")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ConcurrencyStamp).HasColumnType("longtext");

                entity.Property(e => e.Name)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.NormalizedName)
                    .HasMaxLength(256)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Aspnetuserclaims>(entity =>
            {
                entity.ToTable("aspnetuserclaims");

                entity.HasIndex(e => e.UserId)
                    .HasName("IX_AspNetUserClaims_UserId");

                entity.Property(e => e.ClaimType).HasColumnType("longtext");

                entity.Property(e => e.ClaimValue).HasColumnType("longtext");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Aspnetuserclaims)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_AspNetUserClaims_AspNetUsers_UserId");
            });

            modelBuilder.Entity<Aspnetuserlogins>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey })
                    .HasName("PRIMARY");

                entity.ToTable("aspnetuserlogins");

                entity.HasIndex(e => e.UserId)
                    .HasName("IX_AspNetUserLogins_UserId");

                entity.Property(e => e.LoginProvider)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ProviderKey)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ProviderDisplayName).HasColumnType("longtext");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Aspnetuserlogins)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_AspNetUserLogins_AspNetUsers_UserId");
            });

            modelBuilder.Entity<Aspnetuserroles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId })
                    .HasName("PRIMARY");

                entity.ToTable("aspnetuserroles");

                entity.HasIndex(e => e.RoleId)
                    .HasName("IX_AspNetUserRoles_RoleId");

                entity.Property(e => e.UserId)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.RoleId)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Aspnetuserroles)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_AspNetUserRoles_AspNetRoles_RoleId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Aspnetuserroles)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_AspNetUserRoles_AspNetUsers_UserId");
            });

            modelBuilder.Entity<Aspnetusers>(entity =>
            {
                entity.ToTable("aspnetusers");

                entity.HasIndex(e => e.NormalizedEmail)
                    .HasName("EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName)
                    .HasName("UserNameIndex")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ConcurrencyStamp).HasColumnType("longtext");

                entity.Property(e => e.Email)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.EmailConfirmed).HasColumnType("bit(1)");

                entity.Property(e => e.LockoutEnabled).HasColumnType("bit(1)");

                entity.Property(e => e.LockoutEnd).HasColumnType("datetime(6)");

                entity.Property(e => e.NormalizedEmail)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.NormalizedUserName)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.PasswordHash).HasColumnType("longtext");

                entity.Property(e => e.PhoneNumber).HasColumnType("longtext");

                entity.Property(e => e.PhoneNumberConfirmed).HasColumnType("bit(1)");

                entity.Property(e => e.SecurityStamp).HasColumnType("longtext");

                entity.Property(e => e.TwoFactorEnabled).HasColumnType("bit(1)");

                entity.Property(e => e.UserName)
                    .HasMaxLength(256)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Aspnetusertokens>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name })
                    .HasName("PRIMARY");

                entity.ToTable("aspnetusertokens");

                entity.Property(e => e.UserId)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.LoginProvider)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Value).HasColumnType("longtext");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Aspnetusertokens)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_AspNetUserTokens_AspNetUsers_UserId");
            });

            modelBuilder.Entity<EditMedia>(entity =>
            {
                entity.HasKey(e => e.IdEditMedia)
                    .HasName("PRIMARY");

                entity.ToTable("editmedia");

                entity.HasIndex(e => e.IdEdits)
                    .HasName("FK_EditMedia_IdEdits_idx");

                entity.Property(e => e.Blurb)
                    .IsRequired()
                    .HasColumnType("tinytext");

                entity.Property(e => e.Confirmed).HasColumnType("tinyint unsigned");

                entity.Property(e => e.Gore).HasColumnType("tinyint unsigned");

                entity.Property(e => e.MediaType).HasColumnType("tinyint unsigned");

                entity.Property(e => e.SourceFile).IsRequired();

                entity.Property(e => e.SourceFrom).HasColumnType("tinyint unsigned");

                entity.Property(e => e.SubmittedBy)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdEditsNavigation)
                    .WithMany(p => p.EditMedia)
                    .HasForeignKey(d => d.IdEdits)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EditMedia_IdEdits");
            });

            modelBuilder.Entity<Edits>(entity =>
            {
                entity.HasKey(e => e.IdEdits)
                    .HasName("PRIMARY");

                entity.ToTable("edits");

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Confirmed).HasColumnType("tinyint unsigned");

                entity.Property(e => e.Context)
                    .IsRequired()
                    .HasColumnType("mediumtext");

                entity.Property(e => e.Date)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsFixedLength();               

                entity.Property(e => e.Locked).HasColumnType("tinyint unsigned");

                entity.Property(e => e.State).HasColumnType("tinyint unsigned");

                entity.Property(e => e.SubmittedBy)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Weapon).HasColumnType("tinyint unsigned");
               
            });

            modelBuilder.Entity<Flagged>(entity =>
            {
                entity.HasKey(e => e.IdFlagged)
                    .HasName("PRIMARY");

                entity.ToTable("flagged");

                entity.Property(e => e.FlagType).HasColumnType("int unsigned");

                entity.Property(e => e.IdTimelineInfo).HasColumnType("int unsigned");

                entity.Property(e => e.IdUser)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Log>(entity =>
            {
                entity.HasKey(e => e.IdLog)
                    .HasName("PRIMARY");

                entity.ToTable("log");

                entity.Property(e => e.Action).HasColumnType("int unsigned");

                entity.Property(e => e.IdTimelineInfo).HasColumnType("int unsigned");

                entity.Property(e => e.IdUser).HasColumnType("tinytext");

                entity.Property(e => e.IpAddress)
                    .IsRequired()
                    .HasColumnType("tinytext");
            });

            modelBuilder.Entity<Media>(entity =>
            {
                entity.HasKey(e => e.IdMedia)
                    .HasName("PRIMARY");

                entity.ToTable("media");

                entity.HasIndex(e => e.IdTimelineInfo)
                    .HasName("FK_Media_IdTimelineinfo_idx");

                entity.Property(e => e.Blurb)
                    .IsRequired()
                    .HasColumnType("tinytext");

                entity.Property(e => e.Gore).HasColumnType("tinyint unsigned");

                entity.Property(e => e.MediaType).HasColumnType("tinyint unsigned");

                entity.Property(e => e.SourceFile).IsRequired();

                entity.Property(e => e.SourceFrom).HasColumnType("tinyint unsigned");

                entity.Property(e => e.SubmittedBy)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdTimelineinfoNavigation)
                    .WithMany(p => p.Media)
                    .HasForeignKey(d => d.IdTimelineInfo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Media_IdTimelineinfo");
            });

            modelBuilder.Entity<Officers>(entity =>
            {
                entity.HasKey(e => e.IdOfficer)
                    .HasName("PRIMARY");

                entity.ToTable("officers");

                entity.Property(e => e.Name)
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.Race).HasColumnType("tinyint unsigned");

                entity.Property(e => e.Sex).HasColumnType("tinyint unsigned");
            });

            modelBuilder.Entity<Subjects>(entity =>
            {
                entity.HasKey(e => e.IdSubject)
                    .HasName("PRIMARY");

                entity.ToTable("subjects");

                entity.Property(e => e.Name)
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.Race).HasColumnType("tinyint unsigned");

                entity.Property(e => e.Sex).HasColumnType("tinyint unsigned");
            });

            modelBuilder.Entity<Timelineinfo>(entity =>
            {
                entity.HasKey(e => e.IdTimelineInfo)
                    .HasName("PRIMARY");

                entity.ToTable("timelineinfo");

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Context)
                    .IsRequired()
                    .HasColumnType("mediumtext");

                entity.Property(e => e.Date)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Locked).HasColumnType("tinyint unsigned");

                entity.Property(e => e.Misconduct).HasColumnType("tinyint unsigned");

                entity.Property(e => e.State).HasColumnType("tinyint unsigned");

                entity.Property(e => e.SubmittedBy)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Verified).HasColumnType("tinyint unsigned");

                entity.Property(e => e.Weapon).HasColumnType("tinyint unsigned");
            });

            modelBuilder.Entity<TimelineinfoOfficer>(entity =>
            {
                entity.HasKey(e => e.IdTimelineinfoOfficer)
                    .HasName("PRIMARY");

                entity.ToTable("timelineinfo_officer");

                entity.HasIndex(e => e.IdOfficer)
                    .HasName("Junc_FK_TO_Officers_idx");

                entity.HasIndex(e => e.IdTimelineinfo)
                    .HasName("Junc_FK_TO_Timelineinfo_idx");

                entity.Property(e => e.IdTimelineinfoOfficer).HasColumnName("IdTimelineinfo_Officer");

                entity.HasOne(d => d.IdOfficerNavigation)
                    .WithMany(p => p.TimelineinfoOfficer)
                    .HasForeignKey(d => d.IdOfficer)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TO_Officers");

                entity.HasOne(d => d.IdTimelineinfoNavigation)
                    .WithMany(p => p.TimelineinfoOfficer)
                    .HasForeignKey(d => d.IdTimelineinfo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TO_Timelineinfo");
            });

            modelBuilder.Entity<TimelineinfoSubject>(entity =>
            {
                entity.HasKey(e => e.IdTimelineinfoSubject)
                    .HasName("PRIMARY");

                entity.ToTable("timelineinfo_subject");

                entity.HasIndex(e => e.IdSubject)
                    .HasName("Junc_FK_TS_Subjects_idx");

                entity.HasIndex(e => e.IdTimelineinfo)
                    .HasName("Junc_FK_TS_Timelineinfo_idx");

                entity.Property(e => e.IdTimelineinfoSubject).HasColumnName("IdTimelineinfo_Subject");

                entity.HasOne(d => d.IdSubjectNavigation)
                    .WithMany(p => p.TimelineinfoSubject)
                    .HasForeignKey(d => d.IdSubject)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TS_Subjects");

                entity.HasOne(d => d.IdTimelineinfoNavigation)
                    .WithMany(p => p.TimelineinfoSubject)
                    .HasForeignKey(d => d.IdTimelineinfo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TS_Timelineinfo");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
