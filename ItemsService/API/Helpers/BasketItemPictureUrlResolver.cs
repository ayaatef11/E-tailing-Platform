
using API.Dtos;
using AutoMapper;
using Core.Entities.BasketEntites;

namespace API.Helpers
{
    public class BasketItemPictureUrlResolver(IConfiguration configuration) : IValueResolver<BasketItem, BasketItemToReturnDto, string>
    {
      

        public string Resolve(BasketItem source, BasketItemToReturnDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
            {
                return $"{configuration["ApiBaseUrl"]}/{source.PictureUrl}";
            }
            return string.Empty;
        }
    }
}