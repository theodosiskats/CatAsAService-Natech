namespace Models.InfrastructureModels;

public class CatApiResponse
{
    public string? Id { get; set; }
    public string? Url { get; set; }
    public int? Width { get; set; }
    public int? Height { get; set; }
    public List<Breed>? Breeds { get; set; } = [];
}

public class Breed
{
    public Weight? Weight { get; set; }
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? Temperament { get; set; }
    public string? Origin { get; set; }
    public string? CountryCodes { get; set; }
    public string? CountryCode { get; set; }
    public string? Description { get; set; }
    public string? LifeSpan { get; set; }
    public int? Indoor { get; set; }
    public int? Lap { get; set; }
    public string? AltNames { get; set; }
    public int? Adaptability { get; set; }
    public int? AffectionLevel { get; set; }
    public int? ChildFriendly { get; set; }
    public int? DogFriendly { get; set; }
    public int? EnergyLevel { get; set; }
    public int? Grooming { get; set; }
    public int? HealthIssues { get; set; }
    public int? Intelligence { get; set; }
    public int? SheddingLevel { get; set; }
    public int? SocialNeeds { get; set; }
    public int? StrangerFriendly { get; set; }
    public int? Vocalisation { get; set; }
    public int? Experimental { get; set; }
    public int? Hairless { get; set; }
    public int? Natural { get; set; }
    public int? Rare { get; set; }
    public int? Rex { get; set; }
    public int? SuppressedTail { get; set; }
    public int? ShortLegs { get; set; }
    public string? WikipediaUrl { get; set; }
    public int? Hypoallergenic { get; set; }
    public string? ReferenceImageId { get; set; }
}

public class Weight
{
    public string? Imperial { get; set; }
    public string? Metric { get; set; }
}