using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntitiesConfigurations;

public class TagsConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        ApplyConfiguration(builder);
    }

    private void ApplyConfiguration(EntityTypeBuilder<Tag> builder)
    {
        builder.ToTable("Tags");
        
        builder.HasIndex(x => x.Name).IsUnique();
        
        builder
            .HasMany(x => x.Cats)
            .WithMany(y => y.Tags);
    }
}