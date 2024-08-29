using CloudinaryDotNet.Actions;

namespace UserService.services
{
    public interface IPhotoService
    {
        public Task<ImageUploadResult> UploadImageAsync(IFormFile file);
    }
}
