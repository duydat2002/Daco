namespace Daco.Application.Shops.Commands
{
    public record OnboardSellerCommand : IRequest<ResponseDTO>
    {
        [JsonIgnore]
        public Guid UserId { get; init; }
    }
}
