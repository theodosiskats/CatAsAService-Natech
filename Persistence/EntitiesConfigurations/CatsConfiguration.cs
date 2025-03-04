using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntitiesConfigurations;

public class CatsConfiguration : IEntityTypeConfiguration<Cat>
{
    public void Configure(EntityTypeBuilder<Cat> builder)
    {
        ApplyConfiguration(builder);
    }

    private void ApplyConfiguration(EntityTypeBuilder<Cat> builder)
    {
        builder.ToTable("Cats");
        
        builder
            .HasMany(x => x.Tags)
            .WithMany(y => y.Cats);
    }
}