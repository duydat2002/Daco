namespace Daco.Infrastructure.Settings
{
    public class EmailSettings
    {
        public string Host        { get; init; } = null!;
        public int    Port        { get; init; } = 587;
        public string Username    { get; init; } = null!;
        public string Password    { get; init; } = null!;
        public string FromAddress { get; init; } = null!;
        public string FromName    { get; init; } = "Daco";
    }
}
