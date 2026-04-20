namespace Daco.Application.Administration.WithdrawalManagement.Commands
{
    public record CompleteWithdrawalCommand : IRequest<ResponseDTO>
    {
        [JsonIgnore]
        public Guid    WithdrawalId    { get; init; }
        [JsonIgnore]
        public Guid    CompletedBy     { get; init; }
        public string  TransactionCode { get; init; } = null!;
        public string? AdminNote       { get; init; }
    }
}
