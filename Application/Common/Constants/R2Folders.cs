namespace Daco.Application.Common.Constants
{
    public static class R2Folders
    {
        public static string UserAvatar(Guid userId)
            => $"users/{userId}/avatar";

        public static string ProductImages(Guid shopId, Guid productId)
            => $"shops/{shopId}/products/{productId}/images";

        public static string ProductVideos(Guid shopId, Guid productId)
            => $"shops/{shopId}/products/{productId}/videos";

        public static string ShopLogo(Guid shopId)
            => $"shops/{shopId}/logo";

        public static string ShopCover(Guid shopId)
            => $"shops/{shopId}/cover";

        public static string SellerIdentity(Guid sellerId)
            => $"sellers/{sellerId}/identity";

        public static string SellerLicense(Guid sellerId)
            => $"sellers/{sellerId}/license";

        public static string ReviewMedia(Guid reviewId)
            => $"reviews/{reviewId}";

        public static string Temp(Guid userId, string type)
            => $"temp/{userId}/{type}";

        public static string BrandLogo(Guid brandId)
            => $"brands/{brandId}/logo";

        public static string BrandSamples(Guid brandId)
            => $"brands/{brandId}/samples";
    }
}
