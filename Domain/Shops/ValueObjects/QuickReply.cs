namespace Daco.Domain.Shops.ValueObjects
{
    public sealed class QuickReply : ValueObject
    {
        public string Shortcut { get; init; } = null!;  // "/hello"
        public string Text { get; init; } = null!;  // "Xin chào!"

        private QuickReply(string shortcut, string text)
        {
            Shortcut = shortcut;
            Text = text;
        }

        public static QuickReply Create(string shortcut, string text)
        {
            Guard.AgainstNullOrEmpty(shortcut, nameof(shortcut));
            Guard.AgainstNullOrEmpty(text, nameof(text));

            return new QuickReply(shortcut, text);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Shortcut;
            yield return Text;
        }
    }
}
