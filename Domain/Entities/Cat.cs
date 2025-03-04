namespace Domain.Entities;

public class Cat : BaseEntity
{
    public string CatId { get; set; }
    
    public int? ImageId { get; set; }
    public CatImage? Image { get; set; }

    public List<Tag>? Tags { get; set; } = [];
}