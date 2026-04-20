namespace Daco.Application.Administration.ProductManagement.Commands
{
    public record ApproveProductCommand : IRequest<ResponseDTO>
    {
        [JsonIgnore]
        public Guid ProductId  { get; init; }
        [JsonIgnore]
        public Guid ApprovedBy { get; init; }
    }
}
