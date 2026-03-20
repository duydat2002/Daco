namespace Daco.Domain.Administration.Entities
{
    public class AdminRole : Entity
    {
        public string RoleCode { get; private set; }
        public string RoleName { get; private set; }
        public int    Level    { get; private set; }
        public bool   IsActive { get; private set; }
        public bool   IsSystem { get; private set; }

        protected AdminRole() { }
    }
}
