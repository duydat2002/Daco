namespace Daco.Domain.Users.ValueObjects
{
    public sealed class Username : ValueObject
    {
        private static readonly Regex UsernameRegex = new(@"^[a-zA-Z0-9_]{3,50}$", RegexOptions.Compiled);

        public string Value { get; private set; }

        private Username() { } 

        private Username(string username)
        {
            Value = username;
        }

        public static Username Create(string username)
        {
            Guard.AgainstNullOrEmpty(username, nameof(username));

            var normalized = username.Trim().ToLowerInvariant();

            Guard.Against(!UsernameRegex.IsMatch(normalized),
                "Username must be 3-50 characters and can only contain letters, numbers and underscores");

            Guard.Against(IsReservedUsername(normalized),
                "This username is reserved and cannot be used");

            return new Username(normalized);
        }

        private static bool IsReservedUsername(string username)
        {
            var reserved = new[] { "admin", "root", "system", "api", "test", "support" };
            return reserved.Contains(username);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static implicit operator string(Username username) => username.Value;

        public override string ToString() => Value;
    }
}
