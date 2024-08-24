
using OrdersAndItemsService.Core.Entities;


namespace Repository.Data.Configurations
{
    internal class ProductConfigurations : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.Description)
                .IsRequired();

            builder.Property(p => p.PictureUrl)
                .IsRequired();

            builder.Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            builder.HasOne(P => P.Brand)
                .WithMany()
                .HasForeignKey(FK => FK.BrandId);

            builder.HasOne(P => P.Category)
               .WithMany()
               .HasForeignKey(FK => FK.CategoryId); ;
        }
    }
}