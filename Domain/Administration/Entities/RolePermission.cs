namespace Daco.Domain.Administration.Entities
{
    public class RolePermission : Entity
    {
        public Guid     RoleId       { get; private set; }
        public Guid     PermissionId { get; private set; }
        public DateTime CreatedAt    { get; private set; }

        protected RolePermission() { }
    }
}
