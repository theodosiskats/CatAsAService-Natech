namespace Models.DTOs;

public abstract class BaseDto
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
}