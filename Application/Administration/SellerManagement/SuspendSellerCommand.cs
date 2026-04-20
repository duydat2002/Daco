namespace Daco.Application.Administration.SellerManagement
{
    public record SuspendSellerCommand : IRequest<ResponseDTO>
    {
        [JsonIgnore]
        public Guid   SellerId    { get; init; }
        public string Reason      { get; init; }
        [JsonIgnore]
        public Guid   SuspendedBy { get; init; }
    }
}
