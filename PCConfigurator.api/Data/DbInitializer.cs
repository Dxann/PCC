using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using PCConfigurator.API.Models;

namespace PCConfigurator.API.Data
{
    public static class DbInitializer
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
            // Получаем менеджеры через DI
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Роли
            string[] roles = { "Admin", "User" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            // Создаём администратора, если его нет
            string adminUser = "admin";
            string adminPass = "Admin123#@!";

            if (await userManager.FindByNameAsync(adminUser) == null)
            {
                var admin = new ApplicationUser
                {
                    UserName = adminUser,
                    Email = "admin@pcconfigurator.local"
                };

                var result = await userManager.CreateAsync(admin, adminPass);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }
        }
    }
}
