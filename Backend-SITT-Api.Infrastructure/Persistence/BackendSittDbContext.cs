using Backend_SITT_Api.Domain;
using Backend_SITT_Api.Domain.Entityes;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend_SITT_Api.Infrastructure.Persistence
{
    public  class BackendSittDbContext : DbContext
    {
        public readonly IHttpContextAccessor _contextAccessor;

        public DbSet<Tasks> Task { get; set; }
        public BackendSittDbContext(DbContextOptions<BackendSittDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _contextAccessor = httpContextAccessor;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<BaseDomainModel<int>>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate = DateTime.Now;
                        entry.Entity.CreatedBy = entry.Entity.CreatedBy is null ? "system" : entry.Entity.CreatedBy;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedDate = DateTime.Now;
                        entry.Entity.LastModifiedBy = entry.Entity.LastModifiedBy is null ? "system" : entry.Entity.LastModifiedBy;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries<BaseDomainModel<int>>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate = DateTime.Now;
                        entry.Entity.CreatedBy = entry.Entity.CreatedBy is null ? "system" : entry.Entity.CreatedBy;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedDate = DateTime.Now;
                        entry.Entity.LastModifiedBy = entry.Entity.LastModifiedBy is null ? "system" : entry.Entity.LastModifiedBy;
                        break;
                }
            }
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}
