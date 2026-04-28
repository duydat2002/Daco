namespace Daco.Application.Administration.CategoryManagement.Commands
{
    public record AddAttributeValueCommand : IRequest<ResponseDTO>
    {
        [JsonIgnore]
        public Guid         AttributeId { get; init; }
        public List<string> Values      { get; init; } = new();
    }
}
