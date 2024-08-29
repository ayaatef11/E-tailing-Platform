using CloudinaryDotNet.Actions;
using CloudinaryDotNet;

namespace UserService.services
{
    public class CloudinaryService : IPhotoService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService()
        {
            var account = new Account(
               //configurations for your account to load the images 
            );
            _cloudinary = new Cloudinary(account);
        }

        
        public async Task<ImageUploadResult> UploadImageAsync(IFormFile file)
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, file.OpenReadStream())
            };
            return await _cloudinary.UploadAsync(uploadParams);
        }

    }
}
