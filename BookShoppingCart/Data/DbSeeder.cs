using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookShoppingCart.Data
{
    public class DbSeeder
    {
        public static async Task SeedDefaultData(IServiceProvider service)
        {
            ApplicationDbContext context = service.GetService<ApplicationDbContext>();

            // this block will check if there are any pending migrations and apply them
            if ((await context.Database.GetPendingMigrationsAsync()).Count() > 0)
            {
                await context.Database.MigrateAsync();
            }
            UserManager<IdentityUser>? userManager = service.GetService<UserManager<IdentityUser>>();
            RoleManager<IdentityRole>? roleManager = service.GetService<RoleManager<IdentityRole>>();

            // create admin role if not exists
            bool adminRoleExists = await roleManager.RoleExistsAsync(Roles.Admin.ToString());

            if (!adminRoleExists)
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            }

            // create user role if not exists
            bool userRoleExists = await roleManager.RoleExistsAsync(Roles.User.ToString());

            if (!userRoleExists)
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.User.ToString()));
            }

            //Create Admin User
            IdentityUser admin = new IdentityUser
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                EmailConfirmed = true,
            };

            IdentityUser userInDb = await userManager.FindByEmailAsync(admin.Email);
            if (userInDb == null)
            {
                await userManager.CreateAsync(admin, "Admin@123");
                await userManager.AddToRoleAsync(admin, Roles.Admin.ToString());
            }
        }
    }
}
