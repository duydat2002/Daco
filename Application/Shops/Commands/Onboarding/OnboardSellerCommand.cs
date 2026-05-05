namespace Daco.Application.Shops.Commands.Onboarding
{
    public record OnboardSellerCommand : IRequest<ResponseDTO>
    {
        [JsonIgnore]
        public Guid UserId { get; init; }
    }
}
