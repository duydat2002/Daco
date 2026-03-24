namespace Daco.Application.Administration.AdminManagement.Queries
{
    public record GetAdminByIdQuery : IRequest<ResponseDTO>
    {
        public Guid AdminId { get; init; }
    }
}
