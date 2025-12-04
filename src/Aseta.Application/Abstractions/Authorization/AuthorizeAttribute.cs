using Aseta.Domain.Entities.UserRoles;

namespace Aseta.Application.Abstractions.Authorization;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
internal sealed class AuthorizeAttribute : Attribute
{
    public Role Role { get; init; }

    public AuthorizeAttribute(Role role) => Role = role;

    public AuthorizeAttribute() => Role = Role.None;
}