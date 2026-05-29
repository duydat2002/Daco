namespace Daco.Application.Catalog.Categories.Commands
{
    public record DeleteCategoryCommand : IRequest<ResponseDTO>
    {
        [JsonIgnore]
        public Guid CategoryId { get; init; }
        [JsonIgnore]
        public Guid DeletedBy  { get; init; }
    }
}
