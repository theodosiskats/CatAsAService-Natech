namespace Models.DTOs;

public class CatDto : BaseDto
{
    public string CatId { get; set; }
    
    public int? ImageId { get; set; }
    public CatImageDto? Image { get; set; }

    public List<TagDto>? Tags { get; set; } = [];
}