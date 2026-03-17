namespace Daco.Infrastructure.Settings
{
    public class CloudflareR2Settings
    {
        public string AccountId       { get; init; } = null!;
        public string AccessKeyId     { get; init; } = null!;
        public string SecretAccessKey { get; init; } = null!;
        public string BucketName      { get; init; } = null!;
        public string ServiceURL      { get; init; } = null!;
        public string PublicUrl       { get; init; } = null!;
    }
}
