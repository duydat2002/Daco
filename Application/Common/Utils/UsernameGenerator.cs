namespace Daco.Application.Common.Utils
{
    public static class UsernameGenerator
    {
        private static readonly Dictionary<char, string> VietnameseMap = new()
        {
            ['đ'] = "d"
        };

        /// <summary>
        /// Convert tên có dấu sang username không dấu.
        /// 
        /// Examples:
        /// - "Phạm Duy Đạt" → "pham_duy_dat"
        /// - "François Müller" → "francois_muller"
        /// </summary>
        /// <param name="name">Tên có dấu</param>
        /// <param name="useUnderscore">True: dùng underscore ("pham_duy_dat"), False: liền ("phamduydat")</param>
        /// <returns>Username không dấu, chỉ chứa [a-z0-9_]</returns>
        public static string FromName(string name, bool useUnderscore = true)
        {
            if (string.IsNullOrWhiteSpace(name))
                return string.Empty;

            name = Regex.Replace(name.Trim(), @"\s+", " ").ToLowerInvariant();

            var sb = new StringBuilder();
            foreach (var c in name)
            {
                if (VietnameseMap.TryGetValue(c, out var replacement))
                    sb.Append(replacement);
                else
                    sb.Append(c);
            }
            name = sb.ToString();

            name = name.Normalize(NormalizationForm.FormD);
            sb.Clear();

            foreach (var c in name)
            {
                var category = CharUnicodeInfo.GetUnicodeCategory(c);
                if (category != UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }

            name = sb.ToString().Normalize(NormalizationForm.FormC);

            if (useUnderscore)
                name = name.Replace(' ', '_');
            else
                name = name.Replace(" ", "");

            name = Regex.Replace(name, @"[^a-z0-9_]", "");

            if (useUnderscore)
                name = Regex.Replace(name, @"_+", "_");
            name = name.Trim('_');

            return name;
        }

        /// <summary>
        /// Generate unique username từ tên, với suffix nếu cần.
        /// 
        /// Example:
        /// - "Phạm Duy Đạt" → "pham_duy_dat_4782"
        /// - "A" (too short) → "user_a_1234"
        /// </summary>
        /// <param name="name">Tên có dấu</param>
        /// <param name="minLength">Độ dài tối thiểu (default: 3)</param>
        /// <param name="maxLength">Độ dài tối đa (default: 45, chừa chỗ cho suffix _xxxx)</param>
        /// <param name="fallbackPrefix">Prefix nếu tên quá ngắn (default: "user")</param>
        /// <returns>Username với random suffix</returns>
        public static string GenerateWithSuffix(
            string name,
            int minLength = 3,
            int maxLength = 45,
            string fallbackPrefix = "user")
        {
            var username = FromName(name, useUnderscore: true);

            if (string.IsNullOrEmpty(username))
                username = fallbackPrefix;

            if (username.Length < minLength)
                username = $"{fallbackPrefix}_{username}";

            if (username.Length > maxLength)
                username = username[..maxLength];

            var suffix = Random.Shared.Next(1000, 9999);
            return $"{username}_{suffix}";
        }

        /// <summary>
        /// Convert email local part sang username.
        /// "pham.duy.dat@gmail.com" → "pham_duy_dat"
        /// </summary>
        public static string FromEmail(string email, bool useUnderscore = true)
        {
            if (string.IsNullOrWhiteSpace(email))
                return string.Empty;

            var local = email.Split('@')[0];

            if (useUnderscore)
            {
                local = local.Replace('.', '_')
                            .Replace('-', '_');
            }
            else
            {
                local = local.Replace(".", "")
                            .Replace("-", "");
            }

            local = Regex.Replace(local, @"[^a-z0-9_]", "", RegexOptions.IgnoreCase);
            local = local.ToLowerInvariant();

            if (useUnderscore)
                local = Regex.Replace(local, @"_+", "_").Trim('_');

            return local;
        }
    }
}
