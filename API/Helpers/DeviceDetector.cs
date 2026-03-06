namespace Daco.API.Helpers
{
    public static class DeviceDetector
    {
        public static string? Detect(string? userAgent)
        {
            if (string.IsNullOrEmpty(userAgent)) return null;
            var ua = userAgent.ToLowerInvariant();
            if (ua.Contains("mobile") || ua.Contains("android") || ua.Contains("iphone"))
                return "mobile";
            if (ua.Contains("tablet") || ua.Contains("ipad"))
                return "tablet";
            return "desktop";
        }
    }
}
