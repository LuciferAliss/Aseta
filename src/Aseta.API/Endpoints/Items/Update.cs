using Aseta.API.Extensions;
using Aseta.API.Infrastructure;
using Aseta.Application.Abstractions.Authentication;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Application.Items.Update;
using Aseta.Domain.Abstractions.Primitives.Results;

namespace Aseta.API.Endpoints.Items;

internal sealed class Update : IEndpoint
{
    public sealed record Request(CustomFieldValue[] CustomFieldsValue);

    public sealed record CustomFieldValue(string FieldId, string Value);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch("/inventories/{inventoryId}/items/{itemId}", async (
            string inventoryId,
            string itemId,
            Request request,
            ICommandHandler<UpdateItemCommand> handler,
            IUserContext user,
            CancellationToken cancellationToken = default) =>
        {
            _ = Guid.TryParse(itemId, out Guid parsedItemId);
            _ = Guid.TryParse(user.UserId, out Guid userId);
            _ = Guid.TryParse(inventoryId, out Guid parsedInventoryId);

            var customFieldValueData = request.CustomFieldsValue.Select(x =>
            {
                _ = Guid.TryParse(x.FieldId, out Guid fieldId);

                return new CustomFieldValueData(fieldId, x.Value);
            }).ToList();

            var command = new UpdateItemCommand(
                parsedItemId,
                customFieldValueData,
                parsedInventoryId,
                userId);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.Items)
        .RequireAuthorization();
    }
}
