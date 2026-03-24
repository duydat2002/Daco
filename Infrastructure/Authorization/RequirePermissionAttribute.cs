namespace Daco.Infrastructure.Authorization
{
    public class RequirePermissionAttribute : AuthorizeAttribute
    {
        public RequirePermissionAttribute(string permission)
            : base(policy: $"Permission:{permission}")
        {
        }
    }
}
