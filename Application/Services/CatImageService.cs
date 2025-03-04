using Application.Interfaces;

namespace Application.Services;

public class CatImageService(ICatImageStealClient imageStealClient) : ICatImageService
{
    public async Task<byte[]?> DownloadCatImage(string? imageUrl)
    {
        if (imageUrl == null) return null;
        
        try
        {
            return await imageStealClient.DownloadCatImageAsync(imageUrl);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }
}