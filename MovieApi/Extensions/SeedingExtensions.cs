using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MovieApi.Models.Authentication;

namespace MovieApi.Extensions
{
    public static class SeedingExtensions
    {
        public static IHost Seed(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
                var userManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();

                //  Seed roles
                if (!roleManager.RoleExistsAsync(UserRoles.Admin).Result)
                    roleManager.CreateAsync(new IdentityRole(UserRoles.Admin)).Wait();
                if (!roleManager.RoleExistsAsync(UserRoles.User).Result)
                    roleManager.CreateAsync(new IdentityRole(UserRoles.User)).Wait();

                // Seed admin
                var admin = new ApplicationUser
                {
                    UserName = "admin",
                    Email = "admin@admin.hu",
                    EmailConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                string[] roles = { UserRoles.Admin, UserRoles.User };

                if (!userManager.Users.Any(u => u.UserName == admin.UserName))
                {
                    var password = new PasswordHasher<ApplicationUser>();
                    var hashed = password.HashPassword(admin, "asdASD0+");
                    admin.PasswordHash = hashed;

                    userManager.CreateAsync(admin).Wait();
                    userManager.AddToRolesAsync(admin, roles).Wait();
                }
            }
            return host;
        }
    }
}
