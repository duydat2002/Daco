namespace Daco.Domain.Notifications.Constants
{
    public static class NotificationTypes
    {
        //Order
        public static string OrderNew            = "order_new";
        public static string OrderConfirmed      = "order_confirmed";
        public static string OrderShipping       = "order_shipping";
        public static string OrderDelivered      = "order_delivered";
        public static string OrderCancelled      = "order_cancelled";
        public static string OrderReturned       = "order_returned";
        //Product
        public static string ProductLowStock     = "product_low_stock";
        public static string ProductOutOfStock   = "product_out_of_stock";
        public static string ProductReview       = "product_review";
        public static string ProductQuestion     = "product_question";
        //Shop
        public static string ShopNewFollower     = "shop_new_follower";
        public static string ShopViolation       = "shop_violation";
        public static string ShopPenalty         = "shop_penalty";
        //Financial
        public static string PaymentReceived     = "payment_received";
        public static string WithdrawalApproved  = "withdrawal_approved";
        public static string WithdrawalCompleted = "withdrawal_completed";
        //Promotion
        public static string VoucherExpiring     = "voucher_expiring";
        //System
        public static string PolicyUpdate        = "policy_update";
        public static string SystemMaintenance   = "system_maintenance";
        //Chat
        public static string NewMessage          = "new_message";
    }
}
