namespace Daco.Application.Auth.Queries
{
    public record GetAuthProvidersQuery : IRequest<ResponseDTO>
    {
        [JsonIgnore]
        public Guid? UserId { get; init; } = Guid.Empty;
    }
}
