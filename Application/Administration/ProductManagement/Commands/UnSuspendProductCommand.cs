namespace Daco.Application.Administration.ProductManagement.Commands
{
    public record UnSuspendProductCommand : IRequest<ResponseDTO>
    {
        [JsonIgnore]
        public Guid   ProductId     { get; init; }
        public string Reason        { get; init; }
        [JsonIgnore]
        public Guid   UnSuspendedBy { get; init; }
    }
}
