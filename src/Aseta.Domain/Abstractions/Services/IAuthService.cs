using Aseta.Domain.DTO.User;

namespace Aseta.Domain.Abstractions.Services;

public interface IAuthService
{
    Task<UserResponse> GetCurrentUserAsync(Guid userId);
}