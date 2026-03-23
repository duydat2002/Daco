namespace Daco.Application.Administration.AdminManagement
{
    public record AssignAdminRoleCommand : IRequest<ResponseDTO>
    {
        [JsonIgnore]
        public Guid      AdminId           { get; init; }
        public Guid      RoleId            { get; init; }
        public DateTime? ExpiresAt         { get; init; }
        [JsonIgnore] 
        public Guid      AssignedByAdminId { get; init; }
    }
}
