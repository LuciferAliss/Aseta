using System;
using Aseta.Application.Abstractions.Messaging;
using Aseta.Application.Categories.GetAll;
using Aseta.Domain.Abstractions.Persistence;
using Aseta.Domain.Abstractions.Primitives.Results;
using Aseta.Domain.Entities.Inventories;
using Aseta.Domain.Entities.InventoryRoles;
using Aseta.Domain.Entities.Users;
using System.Linq; // Add this using statement for LINQ methods like Distinct()

namespace Aseta.Application.InventoryRoles.GetUser;

internal sealed class GetInventoryUserQueryHandler(IInventoryRepository inventoryRepository, IUserRepository userRepository) : IQueryHandler<GetInventoryUserQuery, UsersResponse>
{
    public async Task<Result<UsersResponse>> Handle(GetInventoryUserQuery query, CancellationToken cancellationToken)
    {
        Inventory? inventory = await inventoryRepository.GetByIdAsync(query.InventoryId, false, cancellationToken, i => i.UserRoles);

        if (inventory is null)
        {
            return InventoryErrors.NotFound(query.InventoryId);
        }

        ICollection<InventoryRole> roles = inventory.UserRoles.Where(x => x.InventoryId == query.InventoryId).ToList();

        var userIds = roles.Select(role => role.UserId).Distinct().ToList();

        IReadOnlyCollection<User> users = await userRepository.FindAsync(u => userIds.Contains(u.Id), cancellationToken: cancellationToken);

        if (users.Any(x => x is null) || users is null)
        {
            return UserErrors.NotFound();
        }

        var response = new UsersResponse(roles.Select(r =>
        {
            User user = users.First(x => x.Id == r.UserId);

            return new UserResponse(user.Id, user.UserName, user.Email, r.Role.ToString());
        })
        .ToList());

        return response;

    }
}
