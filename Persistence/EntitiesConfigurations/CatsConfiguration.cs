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
        
        builder.HasIndex(x => x.CatId).IsUnique();
        
        builder
            .HasMany(x => x.Tags)
            .WithMany(y => y.Cats);

        builder
            .HasOne(x => x.Image)
            .WithOne(y => y.Cat)
            .HasForeignKey<Cat>(x => x.ImageId)
            .IsRequired(false);
    }
}