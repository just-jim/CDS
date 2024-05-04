using CDS.Domain.AssetAggregate.ValueObjects;
using CDS.Domain.ContentDistributionAggregate;
using CDS.Domain.ContentDistributionAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CDS.Infrastructure.Database.Configurations;

public class ContentDistributionConfigurations : IEntityTypeConfiguration<ContentDistribution> {

    public void Configure(EntityTypeBuilder<ContentDistribution> builder) {
        ConfigureContentDistributionsTable(builder);
        ConfigureAssetContentDistributionsTable(builder);
    }

    static void ConfigureContentDistributionsTable(EntityTypeBuilder<ContentDistribution> builder) {
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

    static void ConfigureAssetContentDistributionsTable(EntityTypeBuilder<ContentDistribution> builder) {
        builder.OwnsMany(cd => cd.AssetContentDistributions, acdb => {
            acdb.ToTable("AssetContentDistributions");

            acdb.WithOwner().HasForeignKey("ContentDistributionId");

            acdb.HasKey("Id", "ContentDistributionId");

            acdb.Property(acd => acd.Id)
                .HasConversion(
                    id => id.Value,
                    value => AssetContentDistributionId.Create(value)
                );

            acdb.Property(acd => acd.AssetId)
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => AssetId.Create(value)
                );

            acdb.Property(acd => acd.FileUrl)
                .HasMaxLength(200);

            acdb.HasIndex(acd => acd.AssetId).HasDatabaseName("IX_AssetContentDistributions_AssetId");
        });

        builder.Metadata.FindNavigation(nameof(ContentDistribution.AssetContentDistributions))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}