using System.Net;
using Amazon.S3;
using Application.Interfaces;
using Application.Interfaces.RepositoryInterfaces;
using Domain.Entities;
using Infrastructure.Clients;

namespace Persistence.Repositories;

public class CatsRepository(DataContext context, IAmazonS3 s3, ICatImageService catImageService) : ICatsRepository
{
    public async Task<List<Cat>?> SaveCatsRangeAndImagesAsync(List<Cat> cats)
    {
        try
        {
            foreach (var cat in cats)
            {
                var image = await UploadCatImageToS3(cat);
                cat.Image = image;
            }
        
            context.Cats.AddRange(cats);
            await context.SaveChangesAsync();
            return cats;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            //Delete image from S3
        }
        return null;
    }

    public async Task<CatImage?> UploadCatImageToS3(Cat cat)
    {
        var file = await catImageService.DownloadCatImage(cat.Image?.Url);

        if (file is null)
        {
            cat.Image = null;
            return null;
        }

        var image = new CatImage();
        await using var fileStream = new MemoryStream(file);
        var result = await AmazonS3Service.UploadFileAsync(s3, fileStream, cat.CatId, "image/jpeg");
        if (result.response.HttpStatusCode == HttpStatusCode.OK)
        {
            image.Url = result.fileUrl;
            image.FileName = cat.CatId;
        }

        return image;
    }
}