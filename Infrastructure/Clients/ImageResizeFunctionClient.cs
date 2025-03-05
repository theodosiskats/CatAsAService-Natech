using System.Net.Http.Headers;
using Application.Interfaces;

namespace Infrastructure.Clients
{
    public class ImageResizeFunctionClient(HttpClient httpClient, string functionUrl) : IImageResizeFunctionClient
    {
        public async Task<MemoryStream> CompressImageAsync(
            Stream inputStream, 
            long maxSizeInBytes = 2 * 1024 * 1024)
        {
            try
            {
                using var content = new MultipartFormDataContent();
                
                // Convert image stream to ByteArrayContent
                var imageContent = new StreamContent(inputStream);
                imageContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

                // Attach image as form-data
                content.Add(imageContent, "image", "imageToCompress.jpeg");

                // Send the request to Azure Function
                var response = await httpClient.PostAsync($"{functionUrl}?maxSizeInBytes={maxSizeInBytes}", content);
                response.EnsureSuccessStatusCode();

                // Read and return the compressed image as MemoryStream
                var compressedImageStream = new MemoryStream(await response.Content.ReadAsByteArrayAsync());
                return compressedImageStream;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                
                // Reset stream position and return original image as MemoryStream
                inputStream.Position = 0;
                var originalImageStream = new MemoryStream();
                await inputStream.CopyToAsync(originalImageStream);
                originalImageStream.Position = 0;
                return originalImageStream;
            }
        }
    }
}