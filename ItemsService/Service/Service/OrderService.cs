
using Core.Entities.OrderEntities;
using Core.Entities.ProductEntities;
using Core.interfaces.Repositories;
using Core.Specifications.OrderSpecifications;
using Core.interfaces.Services;

namespace Service.Service
{
    public class OrderService(IUnitOfWork _unitOfWork, IBasketRepository _basketRepository) : IOrderService
    {

        public async Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, OrderAddress shippingAddress)
        {
            var basket = await _basketRepository.GetBasketAsync(basketId);

            var orderitems = new List<OrderItem>();

            if (basket?.Items?.Count > 0)
            {
                foreach (var item in basket.Items)
                {
                    var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);

                    var productItemOrdered = new ProductOrderItem(item.Id, product!.Name, product.PictureUrl);

                    var orderItem = new OrderItem(productItemOrdered, product.Price, item.Quantity);

                    orderitems.Add(orderItem);
                }
            }

            var subTotal = orderitems.Sum(orderItem => orderItem.Price * orderItem.Quantity);

            var deliveryMethod = await _unitOfWork.Repository<OrderDeliveryMethod>().GetByIdAsync(deliveryMethodId);

            var order = new Order(buyerEmail, shippingAddress, deliveryMethod, orderitems, subTotal);

            await _unitOfWork.Repository<Order>().AddAsync(order);

            var result = await _unitOfWork.CompleteAsync();

            if (result <= 0)
                return null;

            return order;
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var ordersRepo = _unitOfWork.Repository<Order>();

            var spec = new OrderSpecifications(buyerEmail);

            var orders = await ordersRepo.GetAllWithSpecAsync(spec);

            return orders;
        }

        public async Task<Order?> GetSpecificOrderForUserAsync(int orderId, string buyerEmail)
        {
            var ordersRepo = _unitOfWork.Repository<Order>();

            var spec = new OrderSpecifications(buyerEmail, orderId);

            var order = await ordersRepo.GetByIdWithSpecAsync(spec);

            return order;
        }

        public async Task<IReadOnlyList<OrderDeliveryMethod>> GetAllDeliveryMethodsAsync()
        {
            var deliveryMethodsRepo = _unitOfWork.Repository<OrderDeliveryMethod>();

            var deliveryMethods = await deliveryMethodsRepo.GetAllAsync();

            return deliveryMethods;
        }
    }
}