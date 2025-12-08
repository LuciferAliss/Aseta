using System;
using System.Windows.Input;
using Aseta.API.Extensions;
using Aseta.API.Infrastructure;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Application.Inventories.Create;
using Aseta.Domain.Abstractions.Primitives.Results;

namespace Aseta.API.Endpoints.Inventories;

internal sealed partial class Create : IEndpoint
{
    public sealed record Request(
        string Name,
        string Description,
        string ImageUrl,
        string CategoryId,
        bool IsPublic,
        string CreatorId);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/inventories/create", async (
            Request request,
            ICommandHandler<CreateInventoryCommand, InventoryResponse> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new CreateInventoryCommand(
                request.Name,
                request.Description,
                request.ImageUrl,
                request.IsPublic,
                request.CategoryId,
                request.CreatorId);

            Result<InventoryResponse> result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.Created, CustomResults.Problem);
        })
        .WithTags(Tags.Inventories)
        .RequireAuthorization();
    }
}