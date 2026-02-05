namespace Daco.Application.Common.Validators
{
    public abstract class BaseValidator<T> : AbstractValidator<T>
    {
        protected void RuleForNotEmpty(
            Expression<Func<T, string?>> expression,
            Action<IRuleBuilderInitial<T, string?>> configure)
        {
            When(x => !string.IsNullOrEmpty(expression.Compile()(x)),
                () => configure(RuleFor(expression)));
        }

        protected void RuleForWhenHasValue<TProperty>(
            Expression<Func<T, TProperty?>> expression,
            Action<IRuleBuilderInitial<T, TProperty>> configure) 
        where TProperty : class
        {
            When(x =>
            {
                var func = expression.Compile();
                var value = func(x);
                return value != null;
            },
            () =>
            {
                configure(RuleFor(expression)!);
            });
        }
    }
}
