using Application.Interfaces;
using Domain.Entities;
using Models.InfrastructureModels;

namespace Application.Services;

public class CatService : ICatService
{
    public List<Cat> ProcessRawFetchedCats(List<CatApiResponse> catsData)
    {
        var parsedCats = new List<Cat>();
        foreach (var cat in catsData)
        {
            parsedCats.Add(new Cat()
            {
                CatId = cat.Id,
            });
            
            //Upload to S3 image and add to cat
            
            //Parse Tags, store those that doesn't exist and add to cat
        }
        
        return parsedCats;
    }
}