using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class FashionLikeSeed
    {
        public static async Task SeedRoles(RoleManager<Role> roleManager, ILoggerFactory loggerFactory)
        {
            try
            {
                if (!roleManager.RoleExistsAsync("Administrator").Result)
                {
                    var role = new Role
                    {
                        Name = "Administrator",
                        NormalizedName = "ADMINISTRATOR",
                        Description = "System Administrator"
                    };

                    await roleManager.CreateAsync(role);
                }

                if (!roleManager.RoleExistsAsync("Viewer").Result)
                {
                    var role = new Role
                    {
                        Name = "Viewer",
                        NormalizedName = "VIEWER",
                        Description = "Regular User"
                    };

                    await roleManager.CreateAsync(role);
                }
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<FashionLikeSeed>();
                logger.LogError(ex.Message);
            }
        }

        public static async Task SeedUsers(UserManager<User> userManager, ILoggerFactory loggerFactory)
        {
            try
            {
                if (userManager.FindByNameAsync("Admin").Result is null)
                {
                    var user = new User
                    {
                        UserName = "Admin",
                        DisplayName = "Admin",
                        Email = "admin@gmail.com",
                        CreationDate = DateTime.Now,
                        Active = true
                    };

                    await userManager.CreateAsync(user, "P@sw0rd*");
                    await userManager.AddToRoleAsync(user, "Administrator");
                }
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<FashionLikeSeed>();
                logger.LogError(ex.Message);
            }
        }
    }
}
