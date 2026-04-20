namespace Daco.Application.Administration.ProductManagement.Commands
{
    public  record SuspendProductCommand : IRequest<ResponseDTO>
    {
        [JsonIgnore]
        public Guid   ProductId   { get; init; }
        public string Reason      { get; init; }
        [JsonIgnore]
        public Guid   SuspendedBy { get; init; }
    }
}
