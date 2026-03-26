namespace Daco.Application.Administration.UserManagement.Queries
{
    public record GetUserTransactionsQuery : IRequest<ResponseDTO>
    {
        [JsonIgnore]
        public Guid      UserId          { get; init; }
        public string?   TransactionType { get; init; }  // topup, spend, refund
        public string?   Status          { get; init; }  // pending, completed, failed
        public DateTime? FromDate        { get; init; }
        public DateTime? ToDate          { get; init; }
        public string?   SortOrder       { get; init; }  // asc, desc
        public int       Page            { get; init; } = 1;
        public int       PageSize        { get; init; } = 20;
    }
}
