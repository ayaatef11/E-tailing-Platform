using CloudinaryDotNet.Actions;
//using OrdersAndItemsService.Models;

namespace BookShop.interfaces
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
        Task<DeletionResult> DeletePhotoAsync(string publicId);
    }
}
