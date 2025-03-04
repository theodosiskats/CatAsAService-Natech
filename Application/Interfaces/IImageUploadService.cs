using Domain.Entities;

namespace Application.Interfaces;

public interface IImageUploadService
{
    public Task<CatImage> UploadImageToS3(byte[] image, string imageName);
}