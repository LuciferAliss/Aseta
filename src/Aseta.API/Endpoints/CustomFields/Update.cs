using System;
using Aseta.API.Extensions;
using Aseta.API.Infrastructure;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Application.CustomFields.Update;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.Entities.Inventories.CustomField;

namespace Aseta.API.Endpoints.CustomFields;

internal sealed class Update : IEndpoint
{
    public sealed record Request(string Name, string Type);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch("/inventories/{id}/custom-fields/{fieldId}", async (
            string id,
            string fieldId,
            Request request,
            ICommandHandler<UpdateCustomFieldDefinitionCommand> handler,
            CancellationToken cancellationToken = default) =>
        {
            _ = Guid.TryParse(id, out Guid inventoryId);
            _ = Guid.TryParse(fieldId, out Guid fieldIdGuid);
            _ = Enum.TryParse(request.Type, out CustomFieldType type);

            var command = new UpdateCustomFieldDefinitionCommand(inventoryId, fieldIdGuid, request.Name, type);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(TagsApi.CustomFields)
        .RequireAuthorization();
    }
}
