using System;
using Aseta.Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;

namespace Aseta.Infrastructure.Database;

public class RoleInitializer
{
    public static async Task InitializeAsync(UserManager<UserApplication> userManager, RoleManager<IdentityRole<Guid>> roleManager)
    {
        string adminEmail = "admin@aseta.com";
        string password = "_Aa123456!";
        
        if (await roleManager.FindByNameAsync("admin") == null)
        {
            await roleManager.CreateAsync(new IdentityRole<Guid>("admin"));
        }
        
        if (await userManager.FindByNameAsync(adminEmail) == null)
        {
            UserApplication admin = new() { Email = adminEmail, UserName = adminEmail, EmailConfirmed = true };
            IdentityResult result = await userManager.CreateAsync(admin, password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, "admin");
            }
        }
    }
}
