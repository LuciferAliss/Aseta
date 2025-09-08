using System;
using Aseta.Application.DTO.User;

namespace Aseta.Application.Abstractions.Services;

public interface IAuthService
{
    Task<UserResponse> GetCurrentUserAsync(Guid userId);
}