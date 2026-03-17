namespace Daco.Application.Users.Commands.Banking
{
    public record UpdateBankAccountCommand : IRequest<ResponseDTO>
    {
        [JsonIgnore]
        public Guid   UserId        { get; init; }
        [JsonIgnore]
        public Guid   BankAccountId { get; init; }
        public string BankCode      { get; init; } = null!;
        public string BankName      { get; init; } = null!;
        public string AccountNumber { get; init; } = null!;
        public string AccountHolder { get; init; } = null!;
    }
}
