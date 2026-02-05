namespace Daco.Application.Common.Validators
{
    public static class PhoneNumberValidator
    {
        public static IRuleBuilderOptions<T, string> ValidPhoneNumber<T>(
        this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .Matches(@"^(\+84|0)[0-9]{9}$").WithMessage("Invalid phone number format");
        }
    }
}
