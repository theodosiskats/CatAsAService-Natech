using Domain.Entities;
using Models.InfrastructureModels;

namespace Application.Interfaces;

public interface ICatService
{
    public Task<Cat?> GetCatById(int id);
    public Task<List<Cat>?> FetchCats();
    public Task<List<Cat>> ProcessRawFetchedCats(List<CatApiResponse> catsData);
    Task<IEnumerable<Cat>> GetCatsAsync(string? tag, int page, int pageSize);
}