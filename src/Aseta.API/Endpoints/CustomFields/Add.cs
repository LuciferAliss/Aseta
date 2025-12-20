using System;
using Aseta.API.Extensions;
using Aseta.API.Infrastructure;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Application.CustomFields.Add;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.Entities.Inventories.CustomField;

namespace Aseta.API.Endpoints.CustomFields;

internal sealed class Add : IEndpoint
{
    public sealed record Request(CustomField[] CustomFields);

    public sealed record CustomField(string Name, string Type);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/inventories/{id}/custom-fields", async (
            string id,
            Request request,
            ICommandHandler<AddCustomFieldDefinitionCommand> handler,
            CancellationToken cancellationToken = default) =>
        {
            _ = Guid.TryParse(id, out Guid inventoryId);

            ICollection<CustomFieldData> customFields = request.CustomFields.Select(c =>
            {
                _ = Enum.TryParse(c.Type, out CustomFieldType type);

                return new CustomFieldData(c.Name, type);
            }).ToList();

            var command = new AddCustomFieldDefinitionCommand(inventoryId, customFields);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.Created, CustomResults.Problem);
        })
        .WithTags(Tags.CustomFields)
        .RequireAuthorization();
    }
}
