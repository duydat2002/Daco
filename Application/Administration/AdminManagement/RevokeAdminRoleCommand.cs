namespace Daco.Application.Administration.AdminManagement
{
    public record RevokeAdminRoleCommand : IRequest<ResponseDTO>
    {
        public Guid AdminId { get; init; }
        public Guid RoleId { get; init; }
        [JsonIgnore]
        public Guid RevokedByAdminId { get; init; }
    }
}
