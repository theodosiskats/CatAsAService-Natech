using Application.Interfaces.RepositoryInterfaces;

namespace Application.Interfaces;

public interface IUnitOfWork
{
    ITagsRepository TagsRepository { get; }
    ICatsRepository CatsRepository { get;}

    Task<bool> Complete();
    bool HasChanges();
}