using CloudinaryDotNet.Actions;
//using OrdersAndItemsService.Models;

namespace OrdersAndItemsService.Services.Iservice
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
        Task<DeletionResult> DeletePhotoAsync(string publicId);
    }
}
