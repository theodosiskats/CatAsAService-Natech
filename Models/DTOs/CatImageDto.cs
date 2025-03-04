namespace Models.DTOs;

public class CatImageDto
{
    public int CatId { get; set; }

    #region ImageProperties
    public int Width { get; set; }
    public int Height { get; set; }
    #endregion

    #region S3Properties
    public string OriginalFilename { get; set; }
    public string FileName { get; set; }
    public string Url { get; set; }
    #endregion
}