namespace Domain.Users.Enums
{
    public enum VerificationStatus
    {
        Pending,
        Verified,
        Expired,
        Failed
    }

    public enum VerificationTokenType
    {
        Email_verification,
        Phone_verification,
        Password_reset
    }
}
