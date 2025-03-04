using Application.Interfaces;
using Domain.Entities;
using Models.InfrastructureModels;

namespace Application.Services;

public class CatService(ICatApiClient catApiClient, IUnitOfWork uow, ICatImageStealClient imageStealClient) : ICatService
{
    public async Task<List<Cat>?> FetchCats()
    {
        var catData = await catApiClient.FetchRandomCats();
        var parsedCats = await ProcessRawFetchedCats(catData);
        return await uow.CatsRepository.SaveCatsRangeAndImagesAsync(parsedCats);
    }

    public async Task<List<Cat>> ProcessRawFetchedCats(List<CatApiResponse> catsData)
    {
        var parsedCats = new List<Cat>();

        // Extract all unique tags from API response
        var allTags = catsData
            .SelectMany(cat => cat.Breeds?
                                   .SelectMany(breed => breed.Temperament?.Split(',', StringSplitOptions.TrimEntries) ?? [])
                               ?? Array.Empty<string>())
            .Distinct()
            .ToList();

        // Fetch existing tags
        var existingTags = await uow.TagsRepository.GetByNamesAsync(allTags);
        var existingTagsDict = existingTags.ToDictionary(t => t.Name, StringComparer.OrdinalIgnoreCase);

        foreach (var cat in catsData)
        {
            var newCat = new Cat { CatId = cat.Id };

            var tags = cat.Breeds?
                .SelectMany(breed => breed.Temperament?.Split(',', StringSplitOptions.TrimEntries) ?? [])
                .Distinct()
                .ToList();

            if (tags == null) continue;

            newCat.Tags = tags.Select(trait =>
                    existingTagsDict.TryGetValue(trait, out var existingTag)
                        ? existingTag // Use existing tag
                        : new Tag { Name = trait } // Create new tag
            ).ToList();

            newCat.Image = new CatImage
            {
                Url = cat.Url ?? string.Empty,
            };
            
            parsedCats.Add(newCat);
        }

        return parsedCats;
    }
}