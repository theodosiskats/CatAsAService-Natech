using Domain.Entities;
using Persistence.Repositories.Interfaces;

namespace Persistence.Repositories;

public class TagsRepository(DataContext context) : ITagsRepository
{
    public async Task<Tag?> GetByNameAsync(string name)
    {
        
        throw new NotImplementedException();
    }
}