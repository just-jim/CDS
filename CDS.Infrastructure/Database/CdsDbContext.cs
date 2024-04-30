using CDS.Domain.AssetAggregate;
using CDS.Domain.Common.Models;
using CDS.Domain.ContentDistributionAggregate;
using CDS.Domain.OrderAggregate;
using CDS.Infrastructure.Database.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace CDS.Infrastructure.Database;

public class CdsDbContext(
    DbContextOptions<CdsDbContext> options, 
    PublishDomainEventsInterceptor publishDomainEventsInterceptor
    ) : DbContext(options) {
    
    public DbSet<Asset> Assets { get; set; } = null!;
    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<ContentDistribution> ContentDistributions { get; set; } = null!;
    
    override protected void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder
            .Ignore<List<IDomainEvent>>()
            .ApplyConfigurationsFromAssembly(typeof(CdsDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    override protected void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        optionsBuilder.AddInterceptors(publishDomainEventsInterceptor);
        base.OnConfiguring(optionsBuilder);
    }
}