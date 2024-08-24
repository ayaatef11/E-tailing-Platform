﻿using API.Dtos;
using AutoMapper;
using Core.Interfaces.Services;
using OrdersAndItemsService.API.Errors;
using OrdersAndItemsService.Controllers;
using OrdersAndItemsService.Core.Entities.OrderEntities;

using System.Security.Claims;
namespace API.Controllers
{
    [Authorize]
    public class OrderController(IOrderService orderService, IMapper mapper) : BaseApiController
    {
        [ProducesResponseType(typeof(OrderToReturnDto), StatusCodes.Status200OK)]// used to specify the type of response and the HTTP status
                                                                                 // code that a particular action method is expected to return.
                                                                                 // This attribute is commonly used in API controllers to provide
                                                                                 // metadata for API documentation and to help tools like Swagger generate accurate documentation.

        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDto orderDto)
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);// Retrieves the email address of the current user from the claims principal.
            var address = mapper.Map<OrderAddressDto, OrderAddress>(orderDto.ShippingAddress);

            var order = await orderService.CreateOrderAsync(buyerEmail, orderDto.BasketId, orderDto.DeliveryMethodId, address);

            if (order is null)
                return BadRequest(new ApiResponse(400));

            return Ok(mapper.Map<Order, OrderToReturnDto>(order));
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrdersForUser()
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);

            var orders = await orderService.GetOrdersForUserAsync(buyerEmail);

            return Ok(mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDto>>(orders));
        }

        [HttpGet("{orderId}")]
        [ProducesResponseType(typeof(OrderToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OrderToReturnDto>> GetSpecificOrderForUser(int orderId)
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);

            var order = await orderService.GetSpecificOrderForUserAsync(orderId, buyerEmail);

            if (order is null)
                return NotFound(new ApiResponse(404));

            return Ok(mapper.Map<Order, OrderToReturnDto>(order));
        }

        [HttpGet("deliveryMethod")]
        public async Task<ActionResult<IReadOnlyList<OrderDeliveryMethod>>> GetAllDeliveryMethods()
        {
            var deliveryMethods = await orderService.GetAllDeliveryMethodsAsync();

            return Ok(deliveryMethods);
        }
    }
}
