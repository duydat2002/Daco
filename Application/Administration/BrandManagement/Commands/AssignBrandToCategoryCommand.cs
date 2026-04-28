namespace Daco.Application.Administration.BrandManagement.Commands
{
    public record AssignBrandToCategoryCommand : IRequest<ResponseDTO>
    {
        [JsonIgnore]
        public Guid BrandId { get; init; }
        public List<Guid> CategoryIds { get; init; } = new();
    }
}
