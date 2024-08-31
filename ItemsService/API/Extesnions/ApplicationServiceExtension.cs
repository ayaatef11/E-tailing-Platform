using API.Helpers;
using Core.interfaces.Repositories;
using Core.interfaces.Services;
using Repository.Repositories;
using Service;
using Service.Service;
//singelton->one copy over the program
//scoped one copy on each request---general
//transient ->one call over the dependency injection 
namespace API.Extesnions
{
    public static class ApplicationServicesExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection Services)
        {

            Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));

            Services.AddScoped(typeof(IProductService), typeof(ProductService));

            Services.AddAutoMapper(typeof(MappingProfiles));

            Services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));

            Services.AddScoped(typeof(IOrderService), typeof(OrderService));

            Services.AddScoped(typeof(IPaymentService), typeof(PaymentService));

            return Services;
        }
    }
}

