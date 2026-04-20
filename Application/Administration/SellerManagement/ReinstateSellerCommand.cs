namespace Daco.Application.Administration.SellerManagement
{
    public record ReinstateSellerCommand : IRequest<ResponseDTO>
    {
        [JsonIgnore]
        public Guid SellerId     { get; init; }
        [JsonIgnore]
        public Guid ReinstatedBy { get; init; }
    }
}
