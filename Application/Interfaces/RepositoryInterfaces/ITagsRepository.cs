using Domain.Entities;

namespace Application.Interfaces.RepositoryInterfaces;

public interface ITagsRepository
{
    public Task<List<Tag>> GetByNamesAsync(List<string> names);

    public Task AddRangeAsync(List<Tag> tags);
}