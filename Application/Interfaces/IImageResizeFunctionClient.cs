namespace Application.Interfaces;

public interface IImageResizeFunctionClient
{
    public Task<MemoryStream> CompressImageAsync(
        Stream inputStream,
        long maxSizeInBytes = 2 * 1024 * 1024);
}