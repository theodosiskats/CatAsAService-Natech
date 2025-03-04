namespace Application.Interfaces;

public interface ICatImageStealClient
{
    public Task<byte[]> DownloadCatImageAsync(string imageUrl);
}