using Aseta.Application.DTO;
using Aseta.Application.DTO.User;

namespace Aseta.Application.Abstractions.Services;

public interface IAdminService
{
    Task<PaginatedResult<UserAdminViewResponse>> GetUsersAsync(int pageNumber, int pageSize);
    Task BlockUserAsync(Guid userId);
    Task UnblockUserAsync(Guid userId);
    Task GrantAdminRoleAsync(Guid userId);
    Task RevokeAdminRoleAsync(Guid userId);
}