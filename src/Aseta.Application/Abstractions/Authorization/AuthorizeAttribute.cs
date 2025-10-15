using Aseta.Domain.Enums;

namespace Aseta.Application.Abstractions.Authorization;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class AuthorizeAttribute : Attribute
{
    public Role Role { get; set; }
    public Permission Permission { get; set; }
}