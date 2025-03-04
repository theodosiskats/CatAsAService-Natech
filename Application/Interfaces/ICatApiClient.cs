using Models.InfrastructureModels;

namespace Application.Interfaces;

public interface ICatApiClient
{
    public Task<List<CatApiResponse>> FetchRandomCats(int? pageSize = null);
}