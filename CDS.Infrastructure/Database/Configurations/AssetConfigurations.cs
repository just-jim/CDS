using CDS.Domain.AssetAggregate;
using CDS.Domain.AssetAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CDS.Infrastructure.Database.Configurations;

public class AssetConfigurations : IEntityTypeConfiguration<Asset> {

    public void Configure(EntityTypeBuilder<Asset> builder) {
        ConfigureAssetsTable(builder);
        ConfigureBriefingsTable(builder);
    }

    static void ConfigureAssetsTable(EntityTypeBuilder<Asset> builder) {
        builder.ToTable("Assets");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => AssetId.Create(value)
            );

        builder.Property(a => a.Name)
            .HasMaxLength(100);

        builder.Property(a => a.Description)
            .HasMaxLength(500);

        builder.Property(a => a.FileFormat)
            .HasMaxLength(100);

        builder.Property(a => a.FileSize)
            .HasMaxLength(100);

        builder.Property(a => a.Path)
            .HasMaxLength(500);

        builder.HasIndex(a => a.Name).HasDatabaseName("IX_Assets_Name");
    }

    static void ConfigureBriefingsTable(EntityTypeBuilder<Asset> builder) {
        builder.OwnsOne(a => a.Briefing, bb => {
            bb.ToTable("Briefings");

            bb.WithOwner().HasForeignKey("AssetId");

            bb.HasKey("Id", "AssetId");

            bb.Property(b => b.Id)
                .HasConversion(
                    id => id.Value,
                    value => BriefingId.Create(value)
                );

            bb.Property(b => b.CreatedBy)
                .HasMaxLength(100);

            bb.Property(b => b.CreatedDate);
        });

        builder.Metadata.FindNavigation(nameof(Asset.Briefing))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}