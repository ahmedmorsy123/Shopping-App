using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShoppingAppDB.Entities;

namespace ShoppingAppDB.Data.Config
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.OrderDate).IsRequired().HasDefaultValueSql("GETDATE()");
            builder.Property(x => x.TotalPrice).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(x => x.Status).IsRequired().HasDefaultValue("Pending");
            builder.Property(x => x.ShippingAddress).IsRequired(false).HasMaxLength(255);
            builder.Property(x => x.PaymentMethod).IsRequired(false).HasMaxLength(50);

            builder.HasOne(x => x.User).WithMany(x => x.Orders).HasForeignKey(x => x.UserId);

            builder.ToTable("Orders");
        }
    }
}