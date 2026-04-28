namespace Daco.Application.Administration.CategoryManagement.Commands
{
    public record DeleteCategoryCommand : IRequest<ResponseDTO>
    {
        [JsonIgnore]
        public Guid CategoryId { get; init; }
        [JsonIgnore]
        public Guid DeletedBy { get; init; }
    }
}
