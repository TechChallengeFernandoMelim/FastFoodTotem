using FastFoodTotem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FastFoodTotem.Infra.SqlServer.EntitiesMappers;

public class OrderEntityMapper
{
    public static void ConfigureEntity(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OrderEntity>().ToTable("Order");

        modelBuilder.Entity<OrderEntity>(x =>
        {
            x.ToTable("Order");
            x.HasKey(c => c.Id).HasName("OrderId");
            x.Property(c => c.Id).HasColumnName("Id").ValueGeneratedOnAdd();
            x.Property(c => c.CreationDate).HasColumnName("CreationDate");
            x.Property(c => c.InStoreOrderId).HasColumnName("InStoreOrderId");

            x.HasMany(c => c.OrderedItems)
            .WithOne(u => u.Order)
            .HasForeignKey(y => y.OrderId);
        });
    }
}

