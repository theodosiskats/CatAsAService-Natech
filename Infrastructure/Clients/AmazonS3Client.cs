using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.DependencyInjection;
using Models.InfrastructureModels;

namespace Infrastructure.Clients;

public static class AmazonS3Service
{
    private static string? _awsRegion;
    private static string? _accessPointArn;

    public static void AddS3Storage(this IServiceCollection services, AmazonS3Settings settings)
    {
        // Fetch AWS configuration
        _awsRegion = settings.AwsRegion;
        _accessPointArn = settings.AccessPointArn;

        // Configure the AWS SDK for S3
        services.AddSingleton<IAmazonS3>(sp =>
        {
            var config = new AmazonS3Config
            {
                RegionEndpoint = RegionEndpoint.GetBySystemName(_awsRegion)
            };
            return new AmazonS3Client(config);
        });
    }

    public static async Task<(PutObjectResponse response, string fileUrl)> UploadFileAsync(
        IAmazonS3 s3Client, Stream fileStream, string keyName, string contentType)
    {
        if (string.IsNullOrEmpty(_accessPointArn))
            throw new InvalidOperationException("Access Point ARN is not configured.");

        var putRequest = new PutObjectRequest
        {
            BucketName = _accessPointArn,
            Key = keyName,
            InputStream = fileStream,
            ContentType = contentType
        };

        // Generate correct public file URL format for Access Points
        var fileUrl = $"https://{_accessPointArn.Replace(":", "-").Replace("/", "-")}.s3-accesspoint.{_awsRegion}.amazonaws.com/{keyName}";

        var response = await s3Client.PutObjectAsync(putRequest);
        return (response, fileUrl);
    }
}