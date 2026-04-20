namespace Daco.Application.Administration.OrderManagement.Commands
{
    public record AdminRefundOrderCommand : IRequest<ResponseDTO>
    {
        [JsonIgnore]
        public Guid   OrderId { get; init; }
        [JsonIgnore]
        public Guid   AdminId { get; init; }
        public string Reason  { get; init; }
    }
}
