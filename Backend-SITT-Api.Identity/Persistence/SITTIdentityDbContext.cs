using Backend_SITT_Api.Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Backend_SITT_Api.Identity.Persistence
{
    public  class SITTIdentityDbContext :IdentityDbContext<ApplicationUser>
    {
        public SITTIdentityDbContext(DbContextOptions <SITTIdentityDbContext> options) : base(options) { 
            
        }

        public virtual DbSet<RefreshToken>? RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder
                .Entity<ApplicationUser>(ent =>
                {

                    ent.Property(p => p.CreatedDate).HasDefaultValueSql($"getutcdate()");
                    ent.Property(p => p.CreatedBy).HasDefaultValueSql("'system'");

                });
        }

        private void AddControl()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                Type entryType = entry.Entity.GetType();
                switch (entry.State)
                {
                    case EntityState.Added:
                        PropertyInfo? CreatedDate = entryType.GetProperty("CreatedDate");
                        PropertyInfo? CreatedBy = entryType.GetProperty("CreatedBy");
                        if (CreatedDate != null && CreatedDate.PropertyType == typeof(DateTime?))
                            CreatedDate.SetValue(entry.Entity, DateTime.Now, null);
                        if (CreatedBy != null && CreatedBy.PropertyType == typeof(string))
                            CreatedBy.SetValue(entry.Entity, CreatedBy.GetValue(entry.Entity) == null ? "system" : CreatedBy.GetValue(entry.Entity), null);
                        break;
                    case EntityState.Modified:
                        PropertyInfo? LastModifiedDate = entryType.GetProperty("LastModifiedDate");
                        PropertyInfo? LastModifiedBy = entryType.GetProperty("LastModifiedBy");
                        if (LastModifiedDate != null && LastModifiedDate.PropertyType == typeof(DateTime?))
                            LastModifiedDate.SetValue(entry.Entity, DateTime.Now, null);
                        if (LastModifiedBy != null && LastModifiedBy.PropertyType == typeof(string))
                            LastModifiedBy.SetValue(entry.Entity, LastModifiedBy.GetValue(entry.Entity) == null ? "system" : LastModifiedBy.GetValue(entry.Entity), null);
                        break;
                }
            }
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            AddControl();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            AddControl();

            return base.SaveChanges();
        }
    }
}
