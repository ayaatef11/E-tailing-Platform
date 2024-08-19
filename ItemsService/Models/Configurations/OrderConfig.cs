using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OrdersAndItemsService.Models.Configurations
{
    //instead of doing so in the dbcontext class
    public class OrderConfig : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.Property(p => p.Id).IsRequired();
            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Price).HasColumnType("decimal(18,2)");
            builder.Property(p => p.Image).IsRequired();
           /* builder.HasOne(b => b.ProductBrand).WithMany()
            .HasForeignKey(p => p.ProductBrandId);
            builder.HasOne(t => t.ProductType).WithMany()
            .HasForeignKey(p => p.ProductTypeId);*/
        }
    }
}
