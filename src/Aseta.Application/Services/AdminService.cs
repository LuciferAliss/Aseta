using Aseta.Domain.Abstractions.Repository;
using Aseta.Domain.Abstractions.Services;
using Aseta.Domain.DTO;
using Aseta.Domain.DTO.User;
using Aseta.Domain.Entities.Users;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Aseta.Application.Services;

public class AdminService(
    UserManager<UserApplication> userManager,
    IUserRepository userRepository,
    IMapper mapper
) : IAdminService
{
    private readonly UserManager<UserApplication> _userManager = userManager;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IMapper _mapper = mapper;

    public async Task BlockUserAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString())
            ?? throw new Exception("User not found");

        if (await _userManager.IsLockedOutAsync(user))
            throw new Exception("User already blocked");

        await _userManager.SetLockoutEndDateAsync(user, DateTime.UtcNow.AddYears(100));
    }

    public async Task<PaginatedResult<UserAdminViewResponse>> GetUsersAsync(int pageNumber, int pageSize)
    {
        var totalCount = await _userManager.Users.CountAsync();

        var users = await _userRepository.GetUsersPageAsync(pageNumber, pageSize);
        var items = _mapper.Map<List<UserAdminViewResponse>>(users);
        
        return new PaginatedResult<UserAdminViewResponse>(
            items,
            pageNumber,
            pageSize,
            totalCount,
            pageNumber * pageSize < totalCount
        );
    }

    public async Task GrantAdminRoleAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString())
            ?? throw new Exception("User not found");

        if (await _userManager.IsInRoleAsync(user, "Admin"))
            throw new Exception("User already has admin role");

        await _userManager.AddToRoleAsync(user, "Admin");
    }

    public async Task RevokeAdminRoleAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString())
            ?? throw new Exception("User not found");

        if (!await _userManager.IsInRoleAsync(user, "Admin"))
            throw new Exception("User doesn't have admin role");

        await _userManager.RemoveFromRoleAsync(user, "Admin");
    }

    public async Task UnblockUserAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString())
            ?? throw new Exception("User not found");

        if (!await _userManager.IsLockedOutAsync(user))
            throw new Exception("User already unblocked");

        await _userManager.SetLockoutEndDateAsync(user, null);
    }
}
