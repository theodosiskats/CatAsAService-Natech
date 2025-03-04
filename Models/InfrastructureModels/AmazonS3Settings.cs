namespace Models.InfrastructureModels;

public class AmazonS3Settings
{
    public string? AwsAccessKey { get; set; }
    public string? AwsSecretKey { get; set; }
    public string? AwsRegion { get; set; }
    public string? BucketName { get; set; }
}