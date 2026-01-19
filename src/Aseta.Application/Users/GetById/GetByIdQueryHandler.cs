using System;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.Entities.Inventories;
using Aseta.Domain.Entities.Users;

namespace Aseta.Application.Users.GetById;

internal sealed class GetByIdQueryHandler(IUserRepository userRepository) : IQueryHandler<GetByIdQuery, UserResponse>
{
    public async Task<Result<UserResponse>> Handle(GetByIdQuery query, CancellationToken cancellationToken)
    {
        User? user = await userRepository.GetByIdAsync(query.Id, false, cancellationToken, i => i.Inventories);

        if (user is null)
        {
            return UserErrors.NotFound(query.Id.ToString());
        }

        ICollection<InventoryResponse> inventories = user.Inventories.Where(i => i.CreatorId == user.Id).Select<Inventory, InventoryResponse>(i =>
        {
            if (!Uri.TryCreate(i.ImageUrl, UriKind.Absolute, out Uri? imageUrl) && imageUrl is null)
            {
                imageUrl = null;
            }

            return new InventoryResponse(i.Id, i.Name, imageUrl, i.CreatedAt);

        }).ToList();

        return new UserResponse(user.Id, user.UserName, user.Email, user.IsLocked, user.Role.ToString(), user.CreatedAt, inventories);
    }
}
