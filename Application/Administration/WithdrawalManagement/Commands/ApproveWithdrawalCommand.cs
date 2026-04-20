namespace Daco.Application.Administration.WithdrawalManagement.Commands
{
    public record ApproveWithdrawalCommand : IRequest<ResponseDTO>
    {
        [JsonIgnore]
        public Guid    WithdrawalId { get; init; }
        [JsonIgnore]
        public Guid    ApprovedBy   { get; init; }
        public string? AdminNote    { get; init; }
    }
}
