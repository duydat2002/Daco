namespace Daco.Domain.Administration.Entities
{
    public class AdminPermission : Entity
    {
        public string  PermissionName { get; private set; }
        public string  PermissionCode { get; private set; }
        public string  Module         { get; private set; }
        public string? Description    { get; private set; }
        public int     SortOrder      { get; private set; }
        public bool    IsActive       { get; private set; }

        protected AdminPermission() { }
    }
}
