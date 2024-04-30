using CDS.Domain.AssetAggregate.ValueObjects;
using CDS.Domain.OrderAggregate;
using CDS.Domain.OrderAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CDS.Infrastructure.Database.Configurations;

public class OrderConfigurations : IEntityTypeConfiguration<Order> {
    
    public void Configure(EntityTypeBuilder<Order> builder) {
        ConfigureOrdersTable(builder);
        ConfigureAssetOrdersTable(builder);
    }

    static void ConfigureOrdersTable(EntityTypeBuilder<Order> builder) {
        builder.ToTable("Orders");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => OrderId.Create(value));

        builder.Property(o => o.CustomerName)
            .HasMaxLength(100);

        builder.Property(o => o.OrderDate);

        builder.Property(o => o.TotalAssets);
    }
    
    static void ConfigureAssetOrdersTable(EntityTypeBuilder<Order> builder) {
        builder.OwnsMany(o => o.AssetOrders, aob =>
        {
            aob.ToTable("AssetOrders");
    
            aob.WithOwner().HasForeignKey("OrderId");

            aob.HasKey("Id", "OrderId");
            
            aob.Property(ao => ao.Id)
                .HasConversion(
                    id => id.Value,
                    value => AssetOrderId.Create(value)
                );

            aob.Property(ao => ao.AssetId)
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => AssetId.Create(value)
                );
            
            aob.Property(ao => ao.Quantity);
        });
        
        builder.Metadata.FindNavigation(nameof(Order.AssetOrders))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}