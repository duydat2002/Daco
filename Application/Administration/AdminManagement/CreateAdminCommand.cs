namespace Daco.Application.Administration.AdminManagement
{
    public record CreateAdminCommand : IRequest<ResponseDTO>
    {
        // User account
        public string     Username     { get; init; } = null!;
        public string     Email        { get; init; } = null!;
        public string     Password     { get; init; } = null!;
        // Admin info
        public string     EmployeeCode { get; init; } = null!;
        public string?    Department   { get; init; }
        public string?    Position     { get; init; }
        public string?    WorkEmail    { get; init; }
        public string?    WorkPhone    { get; init; }
        public string?    Notes        { get; init; }
        // Roles
        public List<Guid> RoleIds      { get; init; } = new();
        [JsonIgnore] 
        public Guid CreatedByAdminId   { get; init; }
    }
}
