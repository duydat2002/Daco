namespace Daco.Application.Users.Commands.Banking
{
    public record SetDefaultBankAccountCommand : IRequest<ResponseDTO>
    {
        [JsonIgnore]
        public Guid UserId        { get; init; }
        [JsonIgnore]
        public Guid BankAccountId { get; init; }
    }
}
