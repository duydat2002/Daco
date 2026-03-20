namespace Daco.Domain.Users.Constants
{
    public static class VerificationTokenTypes
    {
        public const string EmailVerification = "email_verification";
        public const string PhoneVerification = "phone_verification";
        public const string PasswordReset     = "password_reset";
        public const string AdminTwoFactor    = "admin_two_factor";
    }
}
