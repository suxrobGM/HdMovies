using HdMovies.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace HdMovies.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());
            
            if (context.Movies.Any())        
                return;   // DB has been seeded
            

        }

        public static async void CreateUserRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<UserRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

            var superAdminRole = await roleManager.RoleExistsAsync(Role.SuperAdmin.ToString());
            var adminRole = await roleManager.RoleExistsAsync(Role.Admin.ToString());
            var moderatorRole = await roleManager.RoleExistsAsync(Role.Moderator.ToString());
            var editorRole = await roleManager.RoleExistsAsync(Role.Editor.ToString());

            if (!superAdminRole)
            {
                var roleResult = await roleManager.CreateAsync(new UserRole(Role.SuperAdmin));
            }
            if (!adminRole)
            {
                var roleResult = await roleManager.CreateAsync(new UserRole(Role.Admin));
            }
            if (!moderatorRole)
            {
                var roleResult = await roleManager.CreateAsync(new UserRole(Role.Moderator));
            }
            if (!editorRole)
            {
                var roleResult = await roleManager.CreateAsync(new UserRole(Role.Editor));
            }

            var admin = await userManager.FindByEmailAsync("admin@suxrobgm.net");
            if (admin == null)
            {
                admin = new User()
                {
                    UserName = "admin",
                    Email = "admin@suxrobgm.net",
                };

                await userManager.CreateAsync(admin, "Admin1234");
            }

            var hasAdminRole = await userManager.IsInRoleAsync(admin, Role.SuperAdmin.ToString());

            if (!hasAdminRole)
            {
                await userManager.AddToRoleAsync(admin, Role.SuperAdmin.ToString());
            }
        }
    }
}
