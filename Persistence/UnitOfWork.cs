using Application.Interfaces;

namespace Persistence;

public class UnitOfWork(DataContext context) : IUnitOfWork
{
    // public CatsRepository CatsRepository => new(context);

    public async Task<bool> Complete()
    {
        return await context.SaveChangesAsync() > 0;
    }

    public bool HasChanges()
    {
        return context.ChangeTracker.HasChanges();
    }
}