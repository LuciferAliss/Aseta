using System;
using Aseta.API.Extensions;
using Aseta.API.Infrastructure;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Application.CustomFields.Delete;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.Entities.Inventories.CustomField;
using Microsoft.AspNetCore.Mvc;

namespace Aseta.API.Endpoints.CustomFields;

internal sealed class Delete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/inventories/{id}/custom-fields/{fieldId}", async (
            string id,
            string fieldId,
            ICommandHandler<DeleteCustomFieldDefinitionCommand> handler,
            CancellationToken cancellationToken = default) =>
        {
            _ = Guid.TryParse(id, out Guid inventoryId);
            _ = Guid.TryParse(fieldId, out Guid fieldIdGuid);

            var command = new DeleteCustomFieldDefinitionCommand(fieldIdGuid, inventoryId);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(TagsApi.CustomFields)
        .RequireAuthorization();
    }
}
