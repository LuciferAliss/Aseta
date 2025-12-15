using System;
using Aseta.API.Extensions;
using Aseta.API.Infrastructure;
using Aseta.Application.Abstractions.Authorization;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Application.Items.Create;
using Aseta.Domain.Abstractions.Primitives.Results;

namespace Aseta.API.Endpoints.Items;

internal sealed class Create : IEndpoint
{
    public sealed record CustomFieldValueRequest(
        string FieldId,
        string? Value);

    public sealed record Request(CustomFieldValueRequest[] CustomFieldValueRequests);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/inventories/{InventoryId}/items", async (
            string InventoryId,
            Request request,
            ICommandHandler<CreateItemCommand> handler,
            IUserContext user,
            CancellationToken cancellationToken = default) =>
        {
            _ = Guid.TryParse(InventoryId, out Guid inventoryId);
            _ = Guid.TryParse(user.UserId, out Guid userId);

            ICollection<CustomFieldValueData> customFieldValue = request.CustomFieldValueRequests.Select(c =>
            {
                _ = Guid.TryParse(c.FieldId, out Guid fieldId);
                return new CustomFieldValueData(fieldId, c.Value);
            }).ToList();

            var command = new CreateItemCommand(
                customFieldValue,
                inventoryId,
                userId);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.Created, CustomResults.Problem);
        })
        .WithTags(Tags.Items);
    }
}
