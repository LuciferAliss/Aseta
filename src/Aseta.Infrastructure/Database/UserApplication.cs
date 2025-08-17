using System;
using Microsoft.AspNetCore.Identity;

namespace Aseta.Infrastructure.Database;

public class UserApplication : IdentityUser<Guid>;
