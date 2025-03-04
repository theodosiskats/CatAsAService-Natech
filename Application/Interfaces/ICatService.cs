using Domain.Entities;
using Models.InfrastructureModels;

namespace Application.Interfaces;

public interface ICatService
{
    public List<Cat> ProcessRawFetchedCats(List<CatApiResponse> catsData);
}