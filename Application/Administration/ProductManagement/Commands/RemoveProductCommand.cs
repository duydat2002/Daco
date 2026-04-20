namespace Daco.Application.Administration.ProductManagement.Commands
{
    public record RemoveProductCommand : IRequest<ResponseDTO>
    {
        [JsonIgnore]
        public Guid   ProductId { get; init; }
        public string Reason    { get; init; }
        [JsonIgnore]
        public Guid   RemovedBy { get; init; }
    }
}
