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
        
        services.AddSingleton<ICatApiClient>(_ =>
            new CatApiClient(builder.Configuration["CatApiUrl"]));
        
        services.AddS3Storage(new AmazonS3Settings
            {
                AwsRegion = builder.Configuration["AwsRegion"],
                AccessPointArn = builder.Configuration["AccessPointArn"],
            }
        );

        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}