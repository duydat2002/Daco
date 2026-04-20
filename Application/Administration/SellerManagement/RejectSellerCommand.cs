namespace Daco.Application.Administration.SellerManagement
{
    public record RejectSellerCommand : IRequest<ResponseDTO>
    {
        [JsonIgnore]
        public Guid   SellerId   { get; init; }
        public string Reason     { get; init; }
        [JsonIgnore]
        public Guid   RejectedBy { get; init; }
    }
}
