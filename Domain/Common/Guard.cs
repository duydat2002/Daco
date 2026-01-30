namespace Daco.Domain.Common
{
    public static class Guard
    {
        public static void Against(bool condition, string message)
        {
            if (condition)
                throw new InvalidOperationException(message);
        }

        public static void AgainstNull(object? value, string paramName)
        {
            if (value == null)
                throw new ArgumentNullException(paramName);
        }

        public static void AgainstNullOrEmpty(string? value, string paramName)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Value cannot be null or empty", paramName);
        }

        public static void AgainstNegative(decimal value, string paramName)
        {
            if (value < 0)
                throw new ArgumentException("Value cannot be negative", paramName);
        }

        public static void AgainstNegativeOrZero(decimal value, string paramName)
        {
            if (value <= 0)
                throw new ArgumentException("Value must be greater than zero", paramName);
        }

        public static void AgainstOutOfRange(int value, int min, int max, string paramName)
        {
            if (value < min || value > max)
                throw new ArgumentOutOfRangeException(paramName,
                    $"Value must be between {min} and {max}");
        }
    }
}
