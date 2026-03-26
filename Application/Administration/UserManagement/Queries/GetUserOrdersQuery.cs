namespace Daco.Application.Administration.UserManagement.Queries
{
    public record GetUserOrdersQuery : IRequest<ResponseDTO>
    {
        [JsonIgnore]
        public Guid    UserId    { get; init; }
        public string? Status    { get; init; }  // pending_payment, confirmed, shipping, delivered, completed, cancelled
        public string? SortOrder { get; init; }  // asc, desc
        public int     Page      { get; init; } = 1;
        public int     PageSize  { get; init; } = 20;
    }
}
