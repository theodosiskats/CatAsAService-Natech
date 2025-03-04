using Application.Interfaces;
using Domain.Entities;
using Models.InfrastructureModels;

namespace Application.Services;

public class CatService(ICatApiClient catApiClient, IUnitOfWork uow) : ICatService
{
    public async Task<List<Cat>> FetchCats()
    {
        var catData = await catApiClient.FetchRandomCats();
        var processedCatData = ProcessRawFetchedCats(catData);
    }

    public List<Cat> ProcessRawFetchedCats(List<CatApiResponse> catsData)
    {
        var parsedCats = new List<Cat>();
        foreach (var cat in catsData)
        {
            var newCat = new Cat
            {
                CatId = cat.Id,
            };

            var temperamentTraits = cat.Breeds
                .SelectMany(x => x.Temperament.Split(',', StringSplitOptions.TrimEntries))
                .ToList();

            foreach (var trait in temperamentTraits)
            {
                //check if trait already exists
                
                var traitToAdd = await uow.TagsRepository.
                
                //if yes, add the existing one,
                //if no, add to create
            }
            
        }
        
        return parsedCats;
    }
}