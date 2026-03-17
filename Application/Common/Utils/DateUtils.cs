namespace Daco.Application.Common.Utils
{
    public static class DateUtils
    {
        public static DateTime? ParseToUtc(string? date, string format = "dd/MM/yyyy")
        {
            if (string.IsNullOrEmpty(date)) return null;
            return DateTime.SpecifyKind(
                DateTime.ParseExact(date, format, CultureInfo.InvariantCulture),
                DateTimeKind.Utc);
        }
    }
}
