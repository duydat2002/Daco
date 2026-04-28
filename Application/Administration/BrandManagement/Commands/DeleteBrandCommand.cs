namespace Daco.Application.Administration.BrandManagement.Commands
{
    public record DeleteBrandCommand : IRequest<ResponseDTO>
    {
        [JsonIgnore]
        public Guid BrandId { get; init; }
    }
}
