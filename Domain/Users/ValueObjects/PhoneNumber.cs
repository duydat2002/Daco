using Domain.Common;
using Shared.Common;
using System.Text.RegularExpressions;

namespace Domain.Users.ValueObjects
{
    public class PhoneNumber : ValueObject
    {
        private static readonly Regex PhoneRegex = new(@"^(\+84|0)[0-9]{9}$", RegexOptions.Compiled);

        public string Value { get; private set; }

        private PhoneNumber() { } 

        private PhoneNumber(string phone)
        {
            Value = phone;
        }

        public static PhoneNumber Create(string phone)
        {
            Guard.AgainstNullOrEmpty(phone, nameof(phone));

            var normalized = NormalizePhone(phone);

            Guard.Against(!PhoneRegex.IsMatch(normalized),
                "Invalid phone number format. Must be Vietnamese format: +84xxxxxxxxx or 0xxxxxxxxx");

            return new PhoneNumber(normalized);
        }

        private static string NormalizePhone(string phone)
        {
            var cleaned = Regex.Replace(phone, @"[\s\-\(\)]", "");

            if (cleaned.StartsWith("84") && !cleaned.StartsWith("+84"))
                cleaned = "+" + cleaned;

            return cleaned;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static implicit operator string(PhoneNumber phone) => phone.Value;

        public override string ToString() => Value;
    }
}
