using Domain.Entities;

namespace Application.Interfaces.RepositoryInterfaces;

public interface IS3Repository
{
    Task<CatImage> UploadAsync(byte[] file, string fileName);
}