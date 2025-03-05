using Domain.Entities;

namespace Application.Interfaces.RepositoryInterfaces;

public interface ICatsRepository
{
    Task<Cat?> GetByIdAsync(int id);
    Task<List<Cat>?> SaveCatsRangeAndImagesAsync(List<Cat> cats);
    Task<CatImage?> UploadCatImageToS3(Cat cat, CatImage image);
    Task<List<Cat>> GetByIdsAsync(List<string> allCatsIds);
    Task<IEnumerable<Cat>> GetCatsAsync(string? tag, int page, int pageSize);
}