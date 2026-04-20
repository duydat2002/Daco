namespace Daco.Application.Administration.WithdrawalManagement.Commands
{
    public record RejectWithdrawalCommand : IRequest<ResponseDTO>
    {
        [JsonIgnore]
        public Guid    WithdrawalId   { get; init; }
        [JsonIgnore]                  
        public Guid    RejectedBy     { get; init; }
        public string  RejectedReason { get; init; } = null!;
        public string? AdminNote      { get; init; }
    }
}
