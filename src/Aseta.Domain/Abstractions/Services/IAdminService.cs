using Aseta.Domain.DTO;
using Aseta.Domain.DTO.User;

namespace Aseta.Domain.Abstractions.Services;

public interface IAdminService
{
    Task<PaginatedResult<UserAdminViewResponse>> GetUsersAsync(int pageNumber, int pageSize);
    Task BlockUserAsync(Guid userId);
    Task UnblockUserAsync(Guid userId);
    Task GrantAdminRoleAsync(Guid userId);
    Task RevokeAdminRoleAsync(Guid userId);
}