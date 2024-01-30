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
    public class AppDBContext : IdentityDbContext<User, Role, long,
        IdentityUserClaim<long>, IdentityUserRole<long>, IdentityUserLogin<long>,
        IdentityRoleClaim<long>, UserToken>
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
            modelBuilder.Entity<IdentityUserRole<long>>(entity =>
            {
                entity.ToTable("UserRoles").
                    //in case you chagned the TKey type
                    HasKey(key => new { key.UserId, key.RoleId });
            });

            modelBuilder.Entity<IdentityUserClaim<long>>(entity =>
            {
                entity.ToTable("UserClaims").HasKey(x => x.Id);

            });

            modelBuilder.Entity<IdentityUserLogin<long>>(entity =>
            {
                entity.ToTable("UserLogins").HasKey(key => new { key.ProviderKey, key.LoginProvider });
                //in case you chagned the TKey type
                //entity.HasKey(key => new { key.ProviderKey, key.LoginProvider });       
            });

            modelBuilder.Entity<IdentityRoleClaim<long>>(entity =>
            {
                entity.ToTable("RoleClaims").HasKey(x => x.Id);
            });

            modelBuilder.Entity<UserToken>(entity =>
            {
                entity.ToTable("UserTokens").HasKey(key => new { key.UserId, key.LoginProvider, key.Name });
                //in case you chagned the TKey type
                // entity.HasKey(key => new { key.UserId, key.LoginProvider, key.Name });

            });
        }

        public DbSet<Weather> Weathers { get; set; }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            SetAuditFields();
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void SetAuditFields()
        {
            var modifiedEntries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);
            foreach (var entity in modifiedEntries)
            {
                if (entity.Entity is BaseEntity bentity)
                {
                    switch (entity.State)
                    {
                        case EntityState.Added:
                            bentity.CreatedBy = _currentUserService.GetLoggedInUserId();
                            bentity.CreatedOn = DateTime.UtcNow;
                            break;
                        case EntityState.Modified:
                            bentity.ModifiedBy = _currentUserService.GetLoggedInUserId();
                            bentity.ModifiedOn = DateTime.UtcNow;
                            break;
                    }
                }
            }
        }
    }
}
