using System.Net;
using Amazon.S3;
using Application.Helpers;
using Application.Interfaces;
using Application.Interfaces.RepositoryInterfaces;
using Domain.Entities;
using Infrastructure.Clients;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class CatsRepository(DataContext context, IAmazonS3 s3, ICatImageService catImageService, IImageResizeFunctionClient imageResizeFunctionClient) : ICatsRepository
{
    public async Task<Cat?> GetByIdAsync(int id)
    {
        return await context.Cats
            .Include(x => x.Image)
            .Include(x => x.Tags)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<Cat>?> SaveCatsRangeAndImagesAsync(List<Cat> cats)
    {
        try
        {
            foreach (var cat in cats)
            {
                cat.Image = await UploadCatImageToS3(cat, cat.Image);
            }
        
            context.Cats.AddRange(cats);
            await context.SaveChangesAsync();
            return cats;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            //Delete images from S3
        }
        return null;
    }

    public async Task<CatImage?> UploadCatImageToS3(Cat cat, CatImage? catImage)
    {
        var file = await catImageService.DownloadCatImage(cat.Image?.Url);

        if (file is null || catImage is null)
            return null;
        
        // Load the raw bytes into a stream and compress via an Azure Function
        await using var inputStream = new MemoryStream(file);
        var compressedStream = await imageResizeFunctionClient.CompressImageAsync(inputStream);

        // Upload the compressed stream to S3
        var result = await AmazonS3Service.UploadFileAsync(s3, compressedStream, cat.CatId, "image/jpeg");
        if (result.response.HttpStatusCode == HttpStatusCode.OK)
        {
            catImage.Url = result.fileUrl;
            catImage.FileName = cat.CatId;
        }

        return catImage;
    }

    public async Task<List<Cat>> GetByIdsAsync(List<string> allCatsIds)
    {
        return await context.Cats
            .Where(cat => allCatsIds.Contains(cat.CatId))
            .ToListAsync();
    }

    public async Task<IEnumerable<Cat>> GetCatsAsync(string? tag, int page, int pageSize)
    {
        var query = context.Cats.AsQueryable();

        // If tag is provided, filter on it
        if (!string.IsNullOrEmpty(tag))
        {
            query = query.Where(c => c.Tags.Any(t => t.Name == tag));
        }

        // Apply pagination
        return await query
            .Include(x => x.Image)
            .Include(x => x.Tags)
            .OrderBy(x => x.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
}