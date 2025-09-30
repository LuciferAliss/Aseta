using System;
using FluentValidation;

namespace Aseta.Application.Items.Update;

public class UpdateCommandValidator : AbstractValidator<UpdateCommand>
{
    public UpdateCommandValidator()
    {
        RuleFor(x => x.ItemId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.CustomId).NotEmpty();
        RuleFor(x => x.CustomFieldsValue).NotEmpty();
        RuleFor(x => x.InventoryId).NotEmpty();
    }
}
