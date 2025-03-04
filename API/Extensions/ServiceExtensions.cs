using Application.Interfaces;
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
        
        services.AddScoped<ICatService>();
        
        services.AddSingleton<ICatApiClient>(provider =>
            new CatApiClient(builder.Configuration["CatApiUrl"]));
        
        services.AddS3Storage(new AmazonS3Settings
            {
                AwsRegion = builder.Configuration["AwsRegion"],
                AccessPointArn = builder.Configuration["AccessPointArn"],
            }
        );

        //Repositories
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}