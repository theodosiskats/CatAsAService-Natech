namespace Domain.Entities;

public class Tag : BaseEntity
{
    public string Name { get; set; }
    public List<Cat>? Cats { get; set; } = [];
}