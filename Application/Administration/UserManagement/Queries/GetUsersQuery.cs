namespace Daco.Application.Administration.UserManagement.Queries
{
    public record GetUsersQuery : IRequest<ResponseDTO>
    {
        public string? Search     { get; init; }  // username, email, phone
        public string? Status     { get; init; }  // active, suspended, banned, deleted
        public string? SortBy     { get; init; }  // created_at, username
        public string? SortOrder  { get; init; }  // asc, desc
        public int     Page       { get; init; } = 1;
        public int     PageSize   { get; init; } = 20;
    }
}
