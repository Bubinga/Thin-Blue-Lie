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

        public virtual DbSet<Timelineinfo> Timelineinfo { get; set; }
        public virtual DbSet<Media> Media { get; set; }
        public virtual DbSet<Logins> Logins { get; set; }
        public virtual DbSet<Flagged> Flagged { get; set; }
        public virtual DbSet<Log> Log { get; set; }
        public virtual DbSet<Timelineinfo> Edits { get; set; }
        public virtual DbSet<Media> EditMedia{ get; set; }
        public virtual DbSet<Subject> Subjects { get; set; }
        public virtual DbSet<Officer> Officers { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
  // To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
            //    optionsBuilder.UseMySql("server=localhost;user id=root;password=Holland173$;database=thin-blue-lie", x => x.ServerVersion("8.0.20-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Timelineinfo>(entity =>
            {
                entity.HasKey(e => e.IdTimelineInfo)
                    .HasName("PRIMARY");

                entity.ToTable("timelineinfo");

                entity.Property(e => e.IdTimelineInfo).HasColumnName("idTimelineInfo");

                entity.Property(e => e.Armed)
                    .HasColumnName("Armed")
                    .HasColumnType("TINYINT")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.City)
                    .HasColumnType("varchar(20)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Context)
                    .HasColumnType("MEDIUMTEXT")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Date)
                    .IsRequired()
                    .HasColumnType("CHAR(10)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");             

                entity.Property(e => e.Misconduct)
                    .HasColumnType("TINYINT")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.OfficerName)
                    .HasColumnType("varchar(60)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.OfficerRace)
                    .HasColumnType("TINYINT")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.OfficerSex)
                    .HasColumnType("TINYINT")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.State)
                    .IsRequired()
                    .HasColumnType("TINYINT")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.SubjectName)
                    .HasColumnType("varchar(60)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.SubjectRace)
                    .HasColumnType("TINYINT")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.SubjectSex)
                    .HasColumnType("TINYINT")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");              

                entity.Property(e => e.Weapon)
                    .HasColumnType("TINYINT")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });     
              
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
