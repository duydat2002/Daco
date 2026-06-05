namespace Daco.Application.Catalog.Attributes.Commands
{
    public record AddAttributeValueCommand : IRequest<ResponseDTO>
    {
        [JsonIgnore]
        public Guid         AttributeId { get; init; }
        public List<string> Values      { get; init; } = new();
    }
}
