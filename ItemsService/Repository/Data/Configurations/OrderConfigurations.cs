
using OrdersAndItemsService.Core.Models.OrderEntities;

namespace Repository.Data.Configurations
{
    public class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(o => o.ShippingAddress, SAddress => SAddress.WithOwner());

            builder.Property(o => o.Status)
                .HasConversion(
                OStatus => OStatus.ToString(),
                OStatus => (OrderStatus)Enum.Parse(typeof(OrderStatus), OStatus)//convert enum to string to store in the database
                );//if  was int => then this property will be number (0 | 1 | 2 ..)
            // and if was string => then this property will be (pending | Payment Succeded | Payment Failed)

            builder.Property(p => p.SubTotal)
                .HasColumnType("decimal(18,2)");

            builder.HasOne(O => O.DeliveryMethod)
                .WithMany()
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}