using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntitiesConfigurations;

public class CatImagesConfiguration : IEntityTypeConfiguration<CatImage>
{
    public void Configure(EntityTypeBuilder<CatImage> builder)
    {
        ApplyConfiguration(builder);
    }

    private void ApplyConfiguration(EntityTypeBuilder<CatImage> builder)
    {
        builder.ToTable("Cats");
        
        builder
            .HasOne(x => x.Cat)
            .WithOne(y => y.Image)
            .HasForeignKey<CatImage>(y => y.CatId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}