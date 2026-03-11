namespace Daco.Application.Auth.Queries
{
    public record GetActiveSessionsQuery : IRequest<ResponseDTO>
    {
        [JsonIgnore]
        public Guid? UserId { get; init; }

        [JsonIgnore]
        public string? CurrentToken { get; init; }
    }
}
