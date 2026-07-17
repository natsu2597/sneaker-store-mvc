namespace SneakerStore.Services.Services
{
    public interface ICloudinaryService
    {
        Task<(string ImageUrl, string PublicId)> UploadImageAsync(IFormFile file);
        Task DeleteImageAsync(string publicId);
    }
}