namespace Application.Interfaces;

public interface ICatImageService
{
    Task<byte[]?> DownloadCatImage(string? imageUrl);
}