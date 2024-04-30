using CDS.Domain.ContentDistributionAggregate;
using CDS.Domain.ContentDistributionAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CDS.Infrastructure.Database.Configurations;

public class ContentDistributionConfigurations : IEntityTypeConfiguration<ContentDistribution> {
    
    public void Configure(EntityTypeBuilder<ContentDistribution> builder) {
        ConfigureOrdersTable(builder);
    }

    static void ConfigureOrdersTable(EntityTypeBuilder<ContentDistribution> builder) {
        builder.ToTable("ContentDistributions");

        builder.HasKey(cd => cd.Id);

        builder.Property(cd => cd.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => ContentDistributionId.Create(value)
            );

        builder.Property(cd => cd.DistributionDate);

        builder.Property(cd => cd.DistributionChannel)
            .HasMaxLength(100);

        builder.Property(cd => cd.DistributionMethod)
            .HasMaxLength(100);
    }
}