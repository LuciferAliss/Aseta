using FluentValidation;

namespace Aseta.Application.Inventories.Create;

internal sealed class CreateInventoryCommandValidator : AbstractValidator<CreateInventoryCommand>
{
    public CreateInventoryCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MinimumLength(3).MaximumLength(36);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(1000);
        RuleFor(x => x.ImageUrl).NotEmpty().Must(ValidUrl).WithMessage("ImageUrl must be a valid URL.");
    }

    private bool ValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out _);
    }
}