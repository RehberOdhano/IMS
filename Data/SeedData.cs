using IMS.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IMS.Data
{
    public static class SeedData
    {
        public static async Task Initialize(
            IServiceProvider serviceProvider, 
            string password)
        {
            using(var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                // manager
                var managerUID = await EnsureUser(serviceProvider, "manager@demo.com", password);
                await EnsureRole(serviceProvider, managerUID, Constants.InvoiceManagerRole);
            }
        }

        private static async Task<string> EnsureUser(
            IServiceProvider serviceProvider,
            string userName, string initPw)
        {
            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

            var user = await userManager.FindByNameAsync(userName);

            // if we don't have manager account, then we'll create one...
            if (user == null)
            {
                user = new IdentityUser
                {
                    UserName = userName,
                    Email = userName,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, initPw);
            }

            if (user == null)
                throw new Exception("USER DIDN'T CREATED!");

            // if the account is successfully created
            return user.Id;
        }

        private static async Task<IdentityResult> EnsureRole(
            IServiceProvider serviceProvider, string uid, string role)
        {
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            IdentityResult ir;
            
            // if the role i.e. manager, doesn't exist in the database
            if(await roleManager.RoleExistsAsync(role) == false)
            {
                ir = await roleManager.CreateAsync(new IdentityRole(role));
            }

            // fetching the user
            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

            var user = await userManager.FindByIdAsync(uid);

            if(user == null)
                 throw new Exception("USER DOESN'T EXIST!");

            // assigning the role to the user
            ir = await userManager.AddToRoleAsync(user, role);

            return ir;
        }
    }
}
