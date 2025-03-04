using Amazon.S3;
using Application.Interfaces;
using Application.Interfaces.RepositoryInterfaces;
using Persistence.Repositories;

namespace Persistence;

public class UnitOfWork(DataContext context, IAmazonS3 s3, ICatImageService catImageService) : IUnitOfWork
{
    public ICatsRepository CatsRepository => new CatsRepository(context, s3, catImageService);
    public ITagsRepository TagsRepository => new TagsRepository(context);

    public async Task<bool> Complete()
    {
        return await context.SaveChangesAsync() > 0;
    }

    public bool HasChanges()
    {
        return context.ChangeTracker.HasChanges();
    }
}