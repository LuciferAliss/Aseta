using System;
using Aseta.API.Extensions;
using Aseta.API.Infrastructure;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Application.CustomFields.AddCustomFieldDefinition;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.Entities.Inventories.CustomField;

namespace Aseta.API.Endpoints.CustomFields;

internal sealed class AddCustomFieldDefinition : IEndpoint
{
    public sealed record Request(
        CustomField[] CustomFields
    );

    public sealed record CustomField(string Name, string Type);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/inventories/{InventoryId}/custom-fields", async (
            string InventoryId,
            Request Request,
            ICommandHandler<AddCustomFieldDefinitionCommand> handler) =>
        {
            _ = Guid.TryParse(InventoryId, out Guid inventoryId);

            ICollection<CustomFieldDefinitionData> customFields = Request.CustomFields.Select(c =>
            {
                _ = Enum.TryParse(c.Type, out CustomFieldType type);

                return new CustomFieldDefinitionData(c.Name, type);
            }).ToList();

            var command = new AddCustomFieldDefinitionCommand(inventoryId, customFields);

            Result result = await handler.Handle(command, default);

            return result.Match(Results.Created, CustomResults.Problem);
        })
        .WithTags(Tags.CustomFields)
        .RequireAuthorization();
    }
}
