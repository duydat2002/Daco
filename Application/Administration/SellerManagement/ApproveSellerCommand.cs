namespace Daco.Application.Administration.SellerManagement
{
    public record ApproveSellerCommand : IRequest<ResponseDTO>
    {
        [JsonIgnore]
        public Guid SellerId   { get; init; }
        [JsonIgnore]
        public Guid ApprovedBy { get; init; }
    }
}
