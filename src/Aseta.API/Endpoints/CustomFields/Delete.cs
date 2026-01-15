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
    public sealed record Request(CustomField[] CustomFields);

    public sealed record CustomField(string FieldId, string Name, string Type);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/inventories/{id}/custom-fields", async (
            string id,
            [FromBody] Request request,
            ICommandHandler<DeleteCustomFieldDefinitionCommand> handler,
            CancellationToken cancellationToken = default) =>
        {
            _ = Guid.TryParse(id, out Guid inventoryId);

            var customFields = request.CustomFields.Select(c =>
            {
                _ = Guid.TryParse(c.FieldId, out Guid fieldId);
                _ = Enum.TryParse(c.Type, out CustomFieldType type);

                return new CustomFieldData(fieldId, c.Name, type);
            }).ToList();

            var command = new DeleteCustomFieldDefinitionCommand(customFields, inventoryId);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(TagsApi.CustomFields)
        .RequireAuthorization();
    }
}
