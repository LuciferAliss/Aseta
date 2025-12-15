using System;
using System.Windows.Input;
using Aseta.API.Extensions;
using Aseta.API.Infrastructure;
using Aseta.Application.Abstractions.Authorization;
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
        bool IsPublic);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/inventories", async (
            Request request,
            ICommandHandler<CreateInventoryCommand, InventoryResponse> handler,
            IUserContext user,
            CancellationToken cancellationToken = default) =>
        {
            _ = Uri.TryCreate(request.ImageUrl, UriKind.Absolute, out Uri? imageUrl);
            _ = Guid.TryParse(request.CategoryId, out Guid categoryId);
            _ = Guid.TryParse(user.UserId, out Guid creatorId);

            var command = new CreateInventoryCommand(
                request.Name,
                request.Description,
                imageUrl,
                request.IsPublic,
                categoryId,
                creatorId);

            Result<InventoryResponse> result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.Created, CustomResults.Problem);
        })
        .WithTags(Tags.Inventories)
        .RequireAuthorization();
    }
}