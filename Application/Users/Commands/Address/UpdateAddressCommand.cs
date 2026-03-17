namespace Daco.Application.Users.Commands.Address
{
    public record UpdateAddressCommand : IRequest<ResponseDTO>
    {
        [JsonIgnore]
        public Guid    UserId         { get; init; }
        [JsonIgnore]
        public Guid    AddressId      { get; init; }
        public string  RecipientName  { get; init; } = null!;
        public string  RecipientPhone { get; init; } = null!;
        public string  City           { get; init; } = null!;
        public string  District       { get; init; } = null!;
        public string  Ward           { get; init; } = null!;
        public string  AddressDetail  { get; init; } = null!;
        public string? Label          { get; init; }
        public string  AddressType    { get; init; } = "home"; // home, office, other
        public double? Latitude       { get; init; }
        public double? Longitude      { get; init; }
    }
}
