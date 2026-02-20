using Microsoft.AspNetCore.Identity;

namespace BookShoppingCart.Data
{
    public class DbSeeder
    {
        public static async Task SeedDefaultData(IServiceProvider service)
        {
            UserManager<IdentityUser>? userManager = service.GetService<UserManager<IdentityUser>>();
            RoleManager<IdentityRole>? roleManager = service.GetService<RoleManager<IdentityRole>>();

            //Adding some roles to Db
            await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.User.ToString()));

            //Create Admin User
            IdentityUser admin = new IdentityUser
            {
                UserName = "admin",
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
