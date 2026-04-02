namespace Daco.Application.Common.Validators
{
    public static class DateValidator
    {
        public static IRuleBuilderOptions<T, string> ValidDateFormat<T>(
            this IRuleBuilder<T, string> ruleBuilder,
            string format = "dd/MM/yyyy",
            string? message = null)
        {
            return ruleBuilder
            .Must(date =>
                string.IsNullOrWhiteSpace(date) || // cho phép null/empty nếu optional
                DateTime.TryParseExact(
                    date,
                    format,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out _)
            )
            .WithMessage(message ?? $"Date must be in {format} format");
        }
    }
}
