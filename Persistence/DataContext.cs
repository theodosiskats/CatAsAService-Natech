using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.EntitiesConfigurations;

namespace Persistence;

public sealed class DataContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Cat> Cats { get; set; }
    public DbSet<Tag> Tags { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new CatsConfiguration());
        builder.ApplyConfiguration(new TagsConfiguration());

    }
}