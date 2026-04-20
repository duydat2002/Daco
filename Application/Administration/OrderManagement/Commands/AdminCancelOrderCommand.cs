namespace Daco.Application.Administration.OrderManagement.Commands
{
    public record AdminCancelOrderCommand : IRequest<ResponseDTO>
    {
        [JsonIgnore]
        public Guid   OrderId { get; init; }
        public string Reason  { get; init; }
        [JsonIgnore]
        public Guid   AdminId { get; init; }
    }
}
