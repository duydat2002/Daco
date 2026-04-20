namespace Daco.Application.Administration.WithdrawalManagement.Queries
{
    public class GetWithdrawalsQuery : IRequest<ResponseDTO>
    {
        public string?   Status   { get; init; }   // pending, approved, processing, completed, rejected
        public Guid?     SellerId { get; init; }
        public DateTime? FromDate { get; init; }
        public DateTime? ToDate   { get; init; }
        public int       Page     { get; init; } = 1;
        public int       PageSize { get; init; } = 20;
    }
}
