using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class CatsConfiguration : IEntityTypeConfiguration<Cat>
{
    public void Configure(EntityTypeBuilder<Cat> builder)
    {
        ApplyConfiguration(builder);
    }

    private void ApplyConfiguration(EntityTypeBuilder<Cat> builder)
    {
        builder.ToTable("Cats");
        
        builder.HasIndex(x => x.CatId).IsUnique();
        
        builder
            .HasMany(x => x.Tags)
            .WithMany(y => y.Cats);
    }
}