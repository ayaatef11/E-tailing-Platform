
using API.Dtos;
using AutoMapper;
using OrdersAndItemsService.Core.Entities;
using OrdersAndItemsService.Core.Entities.BasketEntites;
using OrdersAndItemsService.Core.Entities.OrderEntities;



namespace API.Helpers
{
    public class MappingProfiles : Profile
    {
        //  public IConfiguration Configuration { get; }

       // Static Mapping: If all your mappings are static and do not depend on external configuration, you do not need IConfiguration.In this case, your current approach, where you define mappings directly in the constructor, is sufficient.
        //Simple Mapping: For straightforward mappings, such as the ones defined in your MappingProfiles class, IConfiguration is not necessary unless you plan to extend it to include configuration-based logic.
        public MappingProfiles()//constructor
        {
            CreateMap<Product, ProductToReturnDto>()
            .ForMember(F => F.Brand, C => C.MapFrom(S => S.Brand.Name))
            .ForMember(F => F.Category, C => C.MapFrom(S => S.Category.Name))
            .ForMember(d => d.PictureUrl, o => o.MapFrom<ProductPictureUrlResolver>());
            CreateMap<BasketDto, Basket>();
            CreateMap<BasketItemDto, BasketItem>();

            CreateMap<Basket, BasketToReturnDto>();

            CreateMap<BasketItem, BasketItemToReturnDto>()
           .ForMember(d => d.PictureUrl, o => o.MapFrom<BasketItemPictureUrlResolver>());

          

            CreateMap<OrderAddressDto, OrderAddress>();

            CreateMap<Order, OrderToReturnDto>()
                .ForMember(d => d.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod.Name))
                .ForMember(d => d.DeliveryMethodCost, o => o.MapFrom(s => s.DeliveryMethod.Cost));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d => d.ProductId, o => o.MapFrom(s => s.Product.ProductId))
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product.ProductName))
                .ForMember(d => d.PictureUrl, o => o.MapFrom(s => s.Product.PictureUrl))
                .ForMember(d => d.PictureUrl, o => o.MapFrom<OrderItemPictureUrlResolver>());

        }

       
    }
}