namespace Daco.Application.Administration.UserManagement.Queries
{
    public record GetUserByIdQuery : IRequest<ResponseDTO>
    {
        public Guid UserId { get; init; }
    }
}
