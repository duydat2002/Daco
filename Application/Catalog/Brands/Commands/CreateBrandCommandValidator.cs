namespace Daco.Application.Catalog.Brands.Commands
{
    public class CreateBrandCommandValidator : BaseValidator<CreateBrandCommand>
    {
        public CreateBrandCommandValidator()
        {
            RuleFor(x => x.BrandName)
                .NotEmpty().WithMessage("Brand name is required")
                .MaximumLength(255).WithMessage("Brand name must not exceed 255 characters");

            RuleForNotEmpty(x => x.WebsiteUrl, rule => rule
                .MustBeValidUrl().WithMessage("Website URL is invalid"));

            RuleForNotEmpty(x => x.LogoUrl, rule => rule
                .MustBeValidUrl().WithMessage("Logo URL is invalid"));

            RuleFor(x => x.SampleImages)
                .Must(images => images is { Count: >= 1 })
                .WithMessage("Sample images must contain at least one image.");

            RuleFor(x => x.SampleImages)
                .Must(images => images is { Count: <= 10 })
                .WithMessage("Sample images must not exceed 10 items")
                .When(x => x.SampleImages is not null);

            RuleForEach(x => x.SampleImages)
                .MustBeValidUrl().WithMessage("Sample image URL is invalid")
                .When(x => x.SampleImages is { Count: > 0 });
        }
    }
}
