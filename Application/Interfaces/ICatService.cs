using Domain.Entities;
using Models.InfrastructureModels;

namespace Application.Interfaces;

public interface ICatService
{
    public Task<List<Cat>?> FetchCats();
    public Task<List<Cat>> ProcessRawFetchedCats(List<CatApiResponse> catsData);
}