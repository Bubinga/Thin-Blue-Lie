using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using DataAccessLibrary.DataModels;

namespace DataAccessLibrary.DataAccess
{
    public partial class DataContext : DbContext
    {
        public DataContext()
        {
        }

        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

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
                optionsBuilder.UseMySql("DataDB", x => x.ServerVersion("8.0.20-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EditMedia>(entity =>
            {
                entity.HasKey(e => e.IdEditMedia)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.IdEdits)
                    .HasName("FK_EditMedia_IdEdits_idx");

                entity.HasIndex(e => e.SubmittedBy)
                    .HasName("FK_EditMedia_UserId");

                entity.Property(e => e.Blurb)
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.SourceFile)
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.SubmittedBy)
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.HasOne(d => d.IdEditsNavigation)
                    .WithMany(p => p.EditMedia)
                    .HasForeignKey(d => d.IdEdits)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EditMedia_IdEdit");
                entity.HasOne(d => d.SubmittedByNavigation)
                    .WithMany(p => p.EditMedia)
                    .HasForeignKey(d => d.SubmittedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EditMedia_UserId");

            });

            modelBuilder.Entity<Edits>(entity =>
            {
                entity.HasKey(e => e.IdEdits)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.IdTimelineInfo)
                    .HasName("FK_Edits_IdTimelineinfo_idx");

                entity.HasIndex(e => e.SubmittedBy)
                    .HasName("FK_Edits_UserId");

                entity.Property(e => e.City)
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Context)
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Date)
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.SubmittedBy)
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.HasOne(d => d.IdTimelineInfoNavigation)
                    .WithMany(p => p.Edits)
                    .HasForeignKey(d => d.IdTimelineInfo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Edits_IdTimelineinfo");

                entity.HasOne(d => d.SubmittedByNavigation)
                    .WithMany(p => p.Edits)
                    .HasForeignKey(d => d.SubmittedBy)
                    .HasConstraintName("FK_Edits_UserId");
            });

            modelBuilder.Entity<Flagged>(entity =>
            {
                entity.HasKey(e => e.IdFlagged)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.IdTimelineInfo)
                    .HasName("FK_Flagged_IdTimelineInfo_idx");

                entity.HasIndex(e => e.UserId)
                    .HasName("FK_Flagged_UserId");

                entity.Property(e => e.Message)
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.UserId)
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.HasOne(d => d.IdTimelineInfoNavigation)
                    .WithMany(p => p.Flagged)
                    .HasForeignKey(d => d.IdTimelineInfo)
                    .HasConstraintName("FK_Flagged_IdTimelineInfo");

                entity.HasOne(d => d.UserIdNavigation)
                    .WithMany(p => p.Flagged)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Edits_UserId");
            });

            modelBuilder.Entity<Log>(entity =>
            {
                entity.HasKey(e => e.IdLog)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.IdTimelineInfo)
                    .HasName("FK_Log_IdTimelineInfo_idx");

                entity.HasIndex(e => e.UserId)
                    .HasName("FK_Log_UserId");

                entity.Property(e => e.IpAddress)
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.UserId)
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.HasOne(d => d.IdTimelineInfoNavigation)
                    .WithMany(p => p.Log)
                    .HasForeignKey(d => d.IdTimelineInfo)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Log_IdTimelineInfo");

                entity.HasOne(d => d.UserIdNavigation)
                   .WithMany(p => p.Log)
                   .HasForeignKey(d => d.UserId)
                   .HasConstraintName("FK_Edits_UserId");
            });

            modelBuilder.Entity<Media>(entity =>
            {
                entity.HasKey(e => e.IdMedia)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.IdTimelineinfo)
                    .HasName("FK_Media_IdTimelineinfo_idx");

                entity.HasIndex(e => e.SubmittedBy)
                    .HasName("FK_Media_UserId");

                entity.Property(e => e.Blurb)
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.SourceFile)
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.SubmittedBy)
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.HasOne(d => d.IdTimelineinfoNavigation)
                    .WithMany(p => p.Media)
                    .HasForeignKey(d => d.IdTimelineinfo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Media_IdTimelineinfo");

                entity.HasOne(d => d.SubmittedByNavigation)
                   .WithMany(p => p.Media)
                   .HasForeignKey(d => d.SubmittedBy)
                   .HasConstraintName("FK_Edits_UserId");
            });

            modelBuilder.Entity<Officers>(entity =>
            {
                entity.HasKey(e => e.IdOfficer)
                    .HasName("PRIMARY");

                entity.Property(e => e.Name)
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<Subjects>(entity =>
            {
                entity.HasKey(e => e.IdSubject)
                    .HasName("PRIMARY");

                entity.Property(e => e.Name)
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<Timelineinfo>(entity =>
            {
                entity.HasKey(e => e.IdTimelineInfo)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.SubmittedBy)
                    .HasName("FK_TimelineInfo_UserId");

                entity.Property(e => e.City)
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Context)
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Date)
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.SubmittedBy)
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.HasOne(d => d.SubmittedByNavigation)
                   .WithMany(p => p.Timelineinfo)
                   .HasForeignKey(d => d.SubmittedBy)
                   .HasConstraintName("FK_Edits_UserId");
            });

            modelBuilder.Entity<TimelineinfoOfficer>(entity =>
            {
                entity.HasKey(e => e.IdTimelineinfoOfficer)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.IdOfficer)
                    .HasName("Junc_FK_TO_Officers_idx");

                entity.HasIndex(e => e.IdTimelineinfo)
                    .HasName("Junc_FK_TO_Timelineinfo_idx");

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

                entity.HasIndex(e => e.IdSubject)
                    .HasName("Junc_FK_TS_Subjects_idx");

                entity.HasIndex(e => e.IdTimelineinfo)
                    .HasName("Junc_FK_TS_Timelineinfo_idx");

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
