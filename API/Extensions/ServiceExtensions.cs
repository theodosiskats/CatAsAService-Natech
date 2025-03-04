using Application.Interfaces;
using Application.Services;
using Infrastructure.Clients;
using Models.InfrastructureModels;
using Persistence;

namespace API.Extensions;

public static class ServiceExtensions
{
    public static void AddApplicationService(this IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AddCors();
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        
        services.AddScoped<ICatService, CatService>();
        services.AddScoped<ICatImageStealClient, CatImageStealClient>();
        services.AddScoped<ICatImageService, CatImageService>();
        
        services.AddSingleton<ICatApiClient>(_ =>
            new CatApiClient(builder.Configuration["CatApiUrl"], builder.Configuration["CatApiKey"]));
        
        services.AddS3Storage(new AmazonS3Settings
            {
                AwsAccessKey = builder.Configuration["AwsAccessKey"],
                AwsSecretKey = builder.Configuration["AwsSecretKey"],
                AwsRegion = builder.Configuration["AwsRegion"],
                BucketName = builder.Configuration["AwsBucketName"],
            }
        );

        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}