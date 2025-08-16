using Aseta.Application.Users.Contracts;

namespace Aseta.Application.Users.Register;

public interface IRegisterService
{
    public Task RegisterUserAsync(UserRegisterRequest dto, CancellationToken cancellationToken);
}
