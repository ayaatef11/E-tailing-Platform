
using OrdersAndItemsService.Core.Models.OrderEntities;

namespace Repository.Data.Configurations
{
    public class OrderItemConfigurations : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.OwnsOne(orderItem => orderItem.Product, Product => Product.WithOwner());

            builder.Property(p => p.Price)
                .HasColumnType("decimal(18,2)");
        }
    }
}