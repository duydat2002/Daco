namespace Daco.Application.Administration.AdminManagement.Commands
{
    public record UpdateAdminStatusCommand : IRequest<ResponseDTO>
    {
        public Guid    AdminId          { get; init; }
        public string  Status           { get; init; } = null!; // "active" | "inactive" | "suspended"
        public string? Reason           { get; init; }
        [JsonIgnore]
        public Guid    UpdatedByAdminId { get; init; }
    }
}
