using Application.Interfaces.RepositoryInterfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class TagsRepository(DataContext context) : ITagsRepository
{
    public async Task<List<Tag>> GetByNamesAsync(List<string> names)
    {
        return await context.Tags
            .Where(tag => names.Contains(tag.Name))
            .ToListAsync();
    }
    
    public async Task AddRangeAsync(List<Tag> tags)
    {
        await context.Tags.AddRangeAsync(tags);
    }

}