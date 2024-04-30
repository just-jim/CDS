using CDS.Domain.AssetAggregate;
using CDS.Domain.AssetAggregate.ValueObjects;
using CDS.Domain.ContentDistributionAggregate.ValueObjects;
using CDS.Domain.OrderAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CDS.Infrastructure.Database.Configurations;

public class AssetConfigurations : IEntityTypeConfiguration<Asset> {
    
    public void Configure(EntityTypeBuilder<Asset> builder) {
        ConfigureAssetsTable(builder);
        ConfigureAssetOrdersTable(builder);
        ConfigureAssetContentDistributionsTable(builder);
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
    }
    
    static void ConfigureAssetContentDistributionsTable(EntityTypeBuilder<Asset> builder) {
        builder.OwnsMany(a => a.AssetContentDistributions, acdb =>
        {
            acdb.ToTable("AssetContentDistributions");
    
            acdb.WithOwner().HasForeignKey("AssetId");

            acdb.HasKey("Id", "AssetId");
            
            acdb.Property(acd => acd.Id)
                .HasConversion(
                    id => id.Value,
                    value => AssetContentDistributionId.Create(value)
                );
            
            acdb.Property(acd => acd.ContentDistributionId)
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => ContentDistributionId.Create(value)
                );

            acdb.Property(acd => acd.FileUrl)
                .HasMaxLength(200);
        });
        
        builder.Metadata.FindNavigation(nameof(Asset.AssetContentDistributions))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
    
    static void ConfigureAssetOrdersTable(EntityTypeBuilder<Asset> builder) {
        builder.OwnsMany(a => a.AssetOrders, aob =>
        {
            aob.ToTable("AssetOrders");
    
            aob.WithOwner().HasForeignKey("AssetId");

            aob.HasKey("Id", "AssetId");
            
            aob.Property(ao => ao.Id)
                .HasConversion(
                    id => id.Value,
                    value => AssetOrderId.Create(value)
                );

            aob.Property(ao => ao.OrderId)
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => OrderId.Create(value)
                );
            
            aob.Property(ao => ao.Quantity);
        });
        
        builder.Metadata.FindNavigation(nameof(Asset.AssetOrders))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
    
    static void ConfigureBriefingsTable(EntityTypeBuilder<Asset> builder) {
        builder.OwnsOne(a => a.Briefing, bb =>
        {
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