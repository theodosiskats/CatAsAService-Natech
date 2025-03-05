using System.Net;
using Amazon.S3;
using Application.Helpers;
using Application.Interfaces;
using Application.Interfaces.RepositoryInterfaces;
using Domain.Entities;
using Infrastructure.Clients;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class CatsRepository(DataContext context, IAmazonS3 s3, ICatImageService catImageService) : ICatsRepository
{
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
        
        // Load the raw bytes into a stream and compress
        await using var inputStream = new MemoryStream(file);
        var compressedStream = await ImageResizeUtilities.CompressImageAsync(inputStream);

        // Now upload the *compressed* stream
        var result = await AmazonS3Service.UploadFileAsync(s3, compressedStream, cat.CatId, "image/jpeg");
        if (result.response.HttpStatusCode == HttpStatusCode.OK)
        {
            catImage.Url = result.fileUrl;
            catImage.FileName = cat.CatId;
        }

        return catImage;
    }

    public async Task<List<Cat>> GetByIdsAsync(List<string?> allCatsIds)
    {
        return await context.Cats
            .Where(cat => allCatsIds.Contains(cat.CatId))
            .ToListAsync();
    }
}