using Application.Interfaces;
using Domain.Entities;
using Models.InfrastructureModels;

namespace Application.Services;

public class CatService(ICatApiClient catApiClient, IUnitOfWork uow) : ICatService
{
    public async Task<List<Cat>?> FetchCats()
    {
        var catData = await catApiClient.FetchRandomCats();
        var parsedCats = await ProcessRawFetchedCats(catData);
        return await uow.CatsRepository.SaveCatsRangeAndImagesAsync(parsedCats);
    }

    public async Task<List<Cat>> ProcessRawFetchedCats(List<CatApiResponse> catsData)
    {
        #region FilterNewCats
        var allCatsIds = catsData.Select(x => x.Id).ToList();
        var existingCats = await uow.CatsRepository.GetByIdsAsync(allCatsIds);
        var existingCatIds = new HashSet<string>(existingCats.Select(ec => ec.CatId));

        catsData = catsData
            .Where(cat => !existingCatIds.Contains(cat.Id))
            .ToList();
        #endregion

        #region FilterAndSaveNewTags

        // Gather all unique tag names
        var allTagNames = catsData
            .SelectMany(cat => cat.Breeds?
                   .SelectMany(breed => breed.Temperament?.Split(',', StringSplitOptions.TrimEntries) ?? [])
               ?? [])
            .Select(t => t.Trim())
            .Distinct()
            .ToList();

        // Fetch existing Tags
        var existingTags = await uow.TagsRepository.GetByNamesAsync(allTagNames);
        var existingTagsDict = existingTags
            .ToDictionary(t => t.Name, t => t, StringComparer.OrdinalIgnoreCase);

        // Find new tag names
        var newTagNames = allTagNames
            .Where(name => !existingTagsDict.ContainsKey(name))
            .ToList();

        // Create list of new tags
        var newTags = newTagNames
            .Select(name => new Tag { Name = name })
            .ToList();

        if (newTags.Count != 0)
        {
            // Add them to the DB
            await uow.TagsRepository.AddRangeAsync(newTags);
            await uow.Complete();
        }

        // Combine existing tags + newly inserted tags
        var allTags = existingTags
            .Concat(newTags)
            .ToDictionary(t => t.Name, t => t, StringComparer.OrdinalIgnoreCase);

        #endregion

        #region MapFetchedCatsToCatEntities

        // Build up the final list of Cat entities, assigning correct Tag references
        var parsedCats = new List<Cat>();

        foreach (var catData in catsData)
        {
            var newCat = new Cat
            {
                CatId = catData.Id,
                Image = new CatImage
                {
                    Url = catData.Url ?? string.Empty,
                    Width = catData.Width ?? 0,
                    Height = catData.Height ?? 0
                }
            };

            // Extract & normalize tags from this cat’s Breeds
            var tagNames = catData.Breeds?
                .SelectMany(breed => breed.Temperament?
                     .Split(',', StringSplitOptions.TrimEntries) 
                 ?? [])
                .Select(t => t.Trim()) // same normalization
                .Distinct()
                .ToList();

            // If there are no tags, skip or handle accordingly
            if (tagNames == null || tagNames.Count == 0)
            {
                newCat.Tags = [];
                continue;
            }

            // Assign the correct Tag entities from our dictionary
            newCat.Tags = tagNames
                .Select(tagName => allTags[tagName]) 
                .ToList();

            parsedCats.Add(newCat);
        }
        #endregion

        return parsedCats;
    }

}