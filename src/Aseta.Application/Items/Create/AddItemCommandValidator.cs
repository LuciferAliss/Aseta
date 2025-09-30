using Aseta.Application.Items.Create;
using FluentValidation;

namespace Aseta.Application.Items.Add;

public class AddItemCommandValidator : AbstractValidator<AddItemCommand>
{
    public AddItemCommandValidator()
    {
        RuleFor(x => x.InventoryId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
    }
}