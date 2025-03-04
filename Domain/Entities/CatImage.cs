namespace Domain.Entities;

public class CatImage : BaseEntity
{
    public int CatId { get; set; }
    public Cat? Cat { get; set; }

    #region ImageProperties
    public int Width { get; set; }
    public int Height { get; set; }
    #endregion

    #region S3Properties
    public string FileName { get; set; }
    public string Url { get; set; }
    #endregion
}