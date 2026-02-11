namespace Daco.Infrastructure.Persistence.Repositories.Users
{
    internal static class UserDbNames
    {
        public const string GetUserById        = "sp_get_user_by_id";
        public const string FindUserByEmail    = "sp_find_user_by_email";
        public const string FindUserByPhone    = "sp_find_user_by_phone";
        public const string CreateUser         = "sp_create_user";
        public const string UpdateUser         = "sp_update_user";
        public const string CreateAuthProvider = "sp_create_auth_provider";
    }
}
