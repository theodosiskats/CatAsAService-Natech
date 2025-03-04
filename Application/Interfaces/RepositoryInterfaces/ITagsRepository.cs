using Domain.Entities;

namespace Persistence.Repositories.Interfaces;

public interface ITagsRepository
{
    public Task<Tag?> GetByNameAsync(string name);
}