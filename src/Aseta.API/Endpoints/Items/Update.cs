using Aseta.API.Extensions;
using Aseta.API.Infrastructure;
using Aseta.Application.Abstractions.Authorization;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Application.Items.Update;
using Aseta.Domain.Abstractions.Primitives.Results;

namespace Aseta.API.Endpoints.Items;

internal sealed class Update : IEndpoint
{
    public sealed record Request(
        ICollection<CustomFieldValue> CustomFieldsValue);
    public sealed record CustomFieldValue(string FieldId, string Value);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch("/inventories/{InventoryId}/items/{ItemId}", async (
            string InventoryId,
            string ItemId,
            Request request,
            ICommandHandler<UpdateItemCommand> handler,
            IUserContext user,
            CancellationToken cancellationToken = default) =>
        {
            _ = Guid.TryParse(ItemId, out Guid itemId);
            _ = Guid.TryParse(user.UserId, out Guid userId);
            _ = Guid.TryParse(InventoryId, out Guid inventoryId);

            var customFieldValueData = request.CustomFieldsValue.Select(x =>
            {
                _ = Guid.TryParse(x.FieldId, out Guid fieldId);

                return new CustomFieldValueData(fieldId, x.Value);
            }).ToList();

            var command = new UpdateItemCommand(
                itemId,
                customFieldValueData,
                inventoryId,
                userId);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.Items)
        .RequireAuthorization();
    }
}
