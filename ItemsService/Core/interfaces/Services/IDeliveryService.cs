

using Core.Entities.OrderEntities;

namespace Core.interfaces.Services
{
    public  interface IDeliveryService
    {
        Task<IReadOnlyList<OrderDeliveryMethod>> GetAllDeliveryMethodsAsync();
    }
}
