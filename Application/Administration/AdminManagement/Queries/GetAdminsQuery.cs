namespace Daco.Application.Administration.AdminManagement.Queries
{
    public record GetAdminsQuery : IRequest<ResponseDTO>
    {
        public string? Search     { get; init; }  // username, email, employee code
        public string? Status     { get; init; }  // active, inactive, suspended
        public string? Department { get; init; }
        public int     Page       { get; init; } = 1;
        public int     PageSize   { get; init; } = 20;
    }
}
