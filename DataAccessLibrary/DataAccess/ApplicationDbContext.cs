using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using DataAccessLibrary.UserModels;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLibrary.DataAccess
{
    public class ApplicationDbContext
    : IdentityDbContext<ApplicationUser, ApplicationRole, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>(b =>
            {
                b.ToTable("Users");
                b.Property(e => e.Id).HasColumnName("UserId");
            });

            modelBuilder.Entity<IdentityUserClaim<int>>(b =>
            {
                b.ToTable("UserClaims");
                b.Property(e => e.Id).HasColumnName("UserClaimId");
            });

            modelBuilder.Entity<IdentityUserLogin<int>>(b =>
            {
                b.ToTable("UserLogins");
            });

            modelBuilder.Entity<IdentityUserToken<int>>(b =>
            {
                b.ToTable("UserTokens");
            });

            modelBuilder.Entity<ApplicationRole>(b =>
            {
                b.ToTable("Roles");
                b.Property(e => e.Id).HasColumnName("RoleId");
            });

            modelBuilder.Entity<IdentityRoleClaim<int>>(b =>
            {
                b.ToTable("RoleClaims");
            });

            modelBuilder.Entity<IdentityUserRole<int>>(b =>
            {
                b.ToTable("UserRoles");
            });
        }
    }
}
