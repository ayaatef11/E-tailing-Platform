
using OrdersAndItemsService.Core.Models.OrderEntities;

namespace Repository.Data.Configurations
{
    public class OrderDeliveryMethodConfigurations : IEntityTypeConfiguration<OrderDeliveryMethod>
    {
        public void Configure(EntityTypeBuilder<OrderDeliveryMethod> builder)
        {
            builder.Property(P => P.Cost)
                .HasColumnType("decimal(18,2)");
        }
    }
}