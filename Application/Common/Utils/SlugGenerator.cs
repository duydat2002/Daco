namespace Daco.Application.Common.Utils
{
    public static class SlugGenerator
    {
        private static readonly Dictionary<char, string> VietnameseMap = new()
        {
            ['đ'] = "d",
            ['Đ'] = "d"
        };

        public static string FromName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return string.Empty;

            name = name.Trim().ToLowerInvariant();
            name = ApplyVietnameseMap(name);
            name = RemoveDiacritics(name);
            name = Regex.Replace(name, @"\s+", "-");
            name = Regex.Replace(name, @"[^a-z0-9\-]", "");
            name = Regex.Replace(name, @"-+", "-");

            return name.Trim('-');
        }

        public static string WithSuffix(string name, int suffix)
            => $"{FromName(name)}-{suffix}";

        private static string ApplyVietnameseMap(string text)
        {
            var sb = new StringBuilder(text.Length);
            foreach (var c in text)
            {
                sb.Append(VietnameseMap.TryGetValue(c, out var mapped) ? mapped : c.ToString());
            }
            return sb.ToString();
        }

        private static string RemoveDiacritics(string text)
        {
            var normalized = text.Normalize(NormalizationForm.FormD);
            var chars = normalized
                .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                .ToArray();
            return new string(chars).Normalize(NormalizationForm.FormC);
        }
    }
}
