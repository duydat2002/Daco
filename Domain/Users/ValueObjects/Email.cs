namespace Daco.Domain.Users.ValueObjects
{
    public sealed class Email : ValueObject
    {
        private static readonly Regex EmailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public string Value { get; }

        private Email(string value)
        {
            Value = value;
        }

        public static Email Create(string email)
        {
            Guard.AgainstNullOrEmpty(email, nameof(email));

            var normalized = email.Trim().ToLowerInvariant();

            Guard.Against(!EmailRegex.IsMatch(normalized), "Invalid email format");
            Guard.Against(normalized.Length > 100, "Email cannot exceed 100 characters");

            return new Email(normalized);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static implicit operator string(Email email) => email.Value;

        public override string ToString() => Value;
    }
}
