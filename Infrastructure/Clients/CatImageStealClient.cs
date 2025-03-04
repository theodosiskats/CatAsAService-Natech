using Application.Interfaces;

namespace Infrastructure.Clients;

public class CatImageStealClient : ICatImageStealClient
{
    private readonly HttpClient _httpClient = new();
    
    public async Task<byte[]> DownloadCatImageAsync(string imageUrl)
    {
        try
        {
            var response = await _httpClient.GetAsync(imageUrl);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsByteArrayAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to download image from {imageUrl}: {ex.Message}");
        }
    }
}