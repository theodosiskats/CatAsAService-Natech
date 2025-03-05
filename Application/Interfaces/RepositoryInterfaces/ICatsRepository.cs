using Domain.Entities;

namespace Application.Interfaces.RepositoryInterfaces;

public interface ICatsRepository
{
    Task<List<Cat>?> SaveCatsRangeAndImagesAsync(List<Cat> cats);
    Task<CatImage?> UploadCatImageToS3(Cat cat, CatImage image);
    Task<List<Cat>> GetByIdsAsync(List<string?> allCatsIds);
}