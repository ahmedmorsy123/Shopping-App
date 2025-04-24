using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShoppingAppDB.Entities;

namespace ShoppingAppDB.Data.Config
{
    public class CartConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.CreatedAt).IsRequired().HasDefaultValueSql("GETDATE()");
            builder.Property(x => x.UpdatedAt).IsRequired().HasDefaultValueSql("GETDATE()");

            builder.HasOne(x => x.User).WithOne(x => x.Cart).HasForeignKey<Cart>(x => x.UserId);

            builder.ToTable("Carts");
        }
    }
}
