namespace Daco.Application.Catalog.Attributes.Queries
{
    public class GetCategoryAttributesQuery : IRequest<ResponseDTO>
    {
        public Guid CategoryId { get; init; }
    }
}
