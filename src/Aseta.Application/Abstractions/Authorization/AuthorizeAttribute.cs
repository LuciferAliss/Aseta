using Aseta.Domain.Enums;

namespace Aseta.Application.Abstractions.Authorization;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class AuthorizeAttribute : Attribute
{
    public Role Role { get; init; }

    public AuthorizeAttribute(Role role) => Role = role;

    public AuthorizeAttribute() => Role = Role.None;
}