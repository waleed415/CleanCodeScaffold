using CleanCodeScaffold.Domain.Entities;
using CleanCodeScaffold.Infrastructure.AuditServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanCodeScaffold.Infrastructure.Persistence
{
    public class AppDBContext : IdentityDbContext<User, Role, int>
    {
        private readonly ICurrentUserService _currentUserService;

        public AppDBContext(DbContextOptions<AppDBContext> options, ICurrentUserService currentUserService)
            : base(options)
        {
            _currentUserService = currentUserService;
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable(name: "User");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role").HasKey(x => x.Id);
            });
            modelBuilder.Entity<IdentityUserRole<int>>(entity =>
            {
                entity.ToTable("UserRoles").
                    //in case you chagned the TKey type
                    HasKey(key => new { key.UserId, key.RoleId });
            });

            modelBuilder.Entity<IdentityUserClaim<int>>(entity =>
            {
                entity.ToTable("UserClaims").HasKey(x => x.Id);

            });

            modelBuilder.Entity<IdentityUserLogin<int>>(entity =>
            {
                entity.ToTable("UserLogins").HasKey(key => new { key.ProviderKey, key.LoginProvider });
                //in case you chagned the TKey type
                  //entity.HasKey(key => new { key.ProviderKey, key.LoginProvider });       
            });

            modelBuilder.Entity<IdentityRoleClaim<int>>(entity =>
            {
                entity.ToTable("RoleClaims").HasKey(x => x.Id);
            });

            modelBuilder.Entity<IdentityUserToken<int>>(entity =>
            {
                entity.ToTable("UserTokens").HasKey(key => new { key.UserId, key.LoginProvider, key.Name });
                //in case you chagned the TKey type
                // entity.HasKey(key => new { key.UserId, key.LoginProvider, key.Name });

            });
        }

        public DbSet<Weather> Weathers { get; set; }
    }
}
