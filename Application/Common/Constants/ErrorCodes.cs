namespace Daco.Application.Common.Constants
{
    public static class ErrorCodes
    {
        // Authentication & Authorization
        public static class Auth
        {
            public const string UserAlreadyExists  = "AUTH.USER_ALREADY_EXISTS";
            public const string InvalidCredentials = "AUTH.INVALID_CREDENTIALS";
            public const string EmailNotVerified   = "AUTH.EMAIL_NOT_VERIFIED";
            public const string PhoneNotVerified   = "AUTH.PHONE_NOT_VERIFIED";
            public const string TokenExpired       = "AUTH.TOKEN_EXPIRED";
            public const string TokenInvalid       = "AUTH.TOKEN_INVALID";
            public const string PasswordTooWeak    = "AUTH.PASSWORD_TOO_WEAK";
            public const string AccountSuspended   = "AUTH.ACCOUNT_SUSPENDED";
            public const string AccountBanned      = "AUTH.ACCOUNT_BANNED";
            public const string TooManyRequests    = "AUTH.TOO_MANY_REQUESTS";
            public const string Unauthorized       = "AUTH.UNAUTHORIZED";
        }

        // Users
        public static class User
        {
            public const string NotFound             = "USER.NOT_FOUND";
            public const string InvalidEmail         = "USER.INVALID_EMAIL";
            public const string InvalidPhone         = "USER.INVALID_PHONE";
            public const string InvalidUsername      = "USER.INVALID_USERNAME";
            public const string AlreadyExists        = "USER.ALREADY_EXISTS";
            public const string AddressLimitExceeded = "USER.ADDRESS_LIMIT_EXCEEDED";
        }

        // Admin
        public static class Admin
        {
            public const string NotFound               = "ADMIN.NOT_FOUND";
            public const string AlreadyDefault         = "ADMIN.ALREADY_DEFAULT";
            public const string EmployeeCodeExists     = "ADMIN.EMPLOYEE_CODE_EXISTS";
            public const string RoleNotFound           = "ADMIN.ROLE_NOT_FOUND";
            public const string CannotUpdateSelf       = "ADMIN.CAN_NOT_UPDATE_SELF";
            public const string CannotUpdateSuperAdmin = "ADMIN.CAN_NOT_UPDATE_SUPERADMIN";
            public const string CannotAssignSuperAdmin = "ADMIN.CAN_NOT_ASSIGN_SUPERADMIN";
            public const string RoleAlreadyAssigned    = "ADMIN.ROLE_ALREADY_ASSIGNED";
            public const string RoleNotAssigned        = "ADMIN.ROLE_NOT_ASSIGNED";
            public const string MustHaveAtLeastOneRole = "ADMIN.MUST_HAVE_AT_LEAST_ONE_ROLE";
        }

        // Address
        public static class Address
        {
            public const string NotFound       = "ADDRESS.NOT_FOUND";
            public const string AlreadyDefault = "ADDRESS.ALREADY_DEFAULT";
        }

        // Bank Account
        public static class BankAccount
        {
            public const string NotFound       = "BANK_ACCOUNT.NOT_FOUND";
            public const string LimitExceeded  = "BANK_ACCOUNT.ADDRESS_LIMIT_EXCEEDED";
            public const string AlreadyExists  = "BANK_ACCOUNT.ALREADY_EXISTS";
            public const string AlreadyDefault = "BANK_ACCOUNT.ALREADY_DEFAULT";
        }

        // Products
        public static class Product
        {
            public const string NotFound          = "PRODUCT.NOT_FOUND";
            public const string OutOfStock        = "PRODUCT.OUT_OF_STOCK";
            public const string InsufficientStock = "PRODUCT.INSUFFICIENT_STOCK";
            public const string InvalidPrice      = "PRODUCT.INVALID_PRICE";
            public const string AlreadyExists     = "PRODUCT.ALREADY_EXISTS";
        }

        // Orders
        public static class Order
        {
            public const string NotFound      = "ORDER.NOT_FOUND";
            public const string CannotCancel  = "ORDER.CANNOT_CANCEL";
            public const string AlreadyPaid   = "ORDER.ALREADY_PAID";
            public const string PaymentFailed = "ORDER.PAYMENT_FAILED";
            public const string InvalidStatus = "ORDER.INVALID_STATUS";
        }

        // Wallet
        public static class Wallet
        {
            public const string InsufficientBalance = "WALLET.INSUFFICIENT_BALANCE";
            public const string TransactionFailed   = "WALLET.TRANSACTION_FAILED";
            public const string InvalidAmount       = "WALLET.INVALID_AMOUNT";
        }

        // Shop
        public static class Shop
        {
            public const string NotFound      = "SHOP.NOT_FOUND";
            public const string AlreadyExists = "SHOP.ALREADY_EXISTS";
            public const string Suspended     = "SHOP.SUSPENDED";
            public const string NotVerified   = "SHOP.NOT_VERIFIED";
        }

        // Validation
        public static class Validation
        {
            public const string Failed        = "VALIDATION.FAILED";
            public const string RequiredField = "VALIDATION.REQUIRED_FIELD";
            public const string InvalidFormat = "VALIDATION.INVALID_FORMAT";
        }

        // System
        public static class System
        {
            public const string InternalError      = "SYSTEM.INTERNAL_ERROR";
            public const string ServiceUnavailable = "SYSTEM.SERVICE_UNAVAILABLE";
            public const string RateLimitExceeded  = "SYSTEM.RATE_LIMIT_EXCEEDED";
        }
    }
}
