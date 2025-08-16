using Aseta.Application.Users.Contracts;
using Aseta.Application.Users.Exceptions;
using Aseta.Domain.User;
using Aseta.Domain.User.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace Aseta.Application.Users.Register;

internal sealed class RegisterService(UserManager<User> userManager) : IRegisterService
{
    private readonly UserManager<User> _userManager = userManager;

    public async Task RegisterUserAsync(UserRegisterRequest request, CancellationToken cancellationToken)
    {
        if (await _userManager.FindByEmailAsync(request.Email).WaitAsync(cancellationToken) != null)
            throw new UserAlreadyExistsException($"User with email '{request.Email}' already exists.");

        if (await _userManager.FindByNameAsync(request.UserName).WaitAsync(cancellationToken) != null)
            throw new UserAlreadyExistsException($"User with name '{request.UserName}' already exists.");

        var user = User.Create(request.UserName, request.Email);

        var result = await _userManager.CreateAsync(user, request.Password).WaitAsync(cancellationToken);

        if (!result.Succeeded)
            throw new CustomValidationException(result.Errors.ToDictionary(x => x.Code, x => new[] { x.Description }));
    }
}
