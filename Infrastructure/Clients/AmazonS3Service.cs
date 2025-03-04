using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.DependencyInjection;
using Models.InfrastructureModels;

namespace Infrastructure.Clients;

public static class AmazonS3Service
{
    private static string? _awsAccessKey;
    private static string? _awsSecretKey;
    private static string? _awsRegion;
    private static string? _bucketName;

    public static void AddS3Storage(this IServiceCollection services, AmazonS3Settings settings)
    {
        // Fetch AWS configuration
        _awsAccessKey = settings.AwsAccessKey;
        _awsSecretKey = settings.AwsSecretKey;
        _awsRegion = settings.AwsRegion;
        _bucketName = settings.BucketName;

        // Configure the AWS SDK for S3
        services.AddSingleton<IAmazonS3>(sp =>
        {
            var config = new AmazonS3Config
            {
                RegionEndpoint = RegionEndpoint.GetBySystemName(_awsRegion)
            };
            return new AmazonS3Client(_awsAccessKey, _awsSecretKey, config);
        });
    }

    public static async Task<(PutObjectResponse response, string fileUrl)> UploadFileAsync(
        IAmazonS3 s3Client, Stream fileStream, string keyName, string contentType)
    {
        if (string.IsNullOrEmpty(_bucketName))
            throw new InvalidOperationException("Access Point ARN is not configured.");

        var putRequest = new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = keyName,
            InputStream = fileStream,
            ContentType = contentType
        };

        var fileUrl = $"https://{_bucketName}.s3.amazonaws.com/cats/{keyName}";

        var response = await s3Client.PutObjectAsync(putRequest);
        return (response, fileUrl);
    }
    
    public static async Task<DeleteObjectResponse> DeleteFileAsync(IAmazonS3 s3Client, string keyName)
    {
        var deleteRequest = new DeleteObjectRequest
        {
            BucketName = _bucketName,
            Key = keyName
        };

        var response = await s3Client.DeleteObjectAsync(deleteRequest);
        return response;
    }
}