namespace Daco.Infrastructure.Persistence.Mappings.Users
{
    public class UserStatusMapper
    {
        public static UserStatus FromDb(string value)
            => value?.ToLower() switch
            {
                "active"    => UserStatus.Active,
                "suspended" => UserStatus.Suspended,
                "banned"    => UserStatus.Banned,
                "deleted"   => UserStatus.Deleted,
                _           => throw new ArgumentOutOfRangeException(nameof(value))
            };

        public static string ToDb(UserStatus status)
            => status switch
            {
                UserStatus.Active    => "active",
                UserStatus.Suspended => "suspended",
                UserStatus.Banned    => "banned",
                UserStatus.Deleted   => "deleted",
                _                    => throw new ArgumentOutOfRangeException(nameof(status))
            };
    }
}
