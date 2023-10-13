using Identity.Infrastructure.Extensions;
using Identity.Shared;
using Identity.Shared.Models;
using Identity.Shared.Permissions;
using Microsoft.AspNetCore.Identity;

namespace Identity.Infrastructure.Seeder;

public class DatabaseSeeder : IDatabaseSeeder
{
    private readonly IndentityDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public DatabaseSeeder(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        IndentityDbContext db)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _db = db;
    }

    public void Initialize()
    {
        AddAdministrator();
        AddBasicUser();
        _db.SaveChanges();
    }

    private void AddAdministrator()
    {
        Task.Run(async () =>
        {
            var adminRole = new ApplicationRole
            {
                Name = ApplicationConstans.RoleConstants.AdministratorRole,
                Description= "Administrator role with full permissions",
            };         
            var adminRoleInDb = await _roleManager.FindByNameAsync(ApplicationConstans.RoleConstants.AdministratorRole);
            if (adminRoleInDb == null)
            {
                await _roleManager.CreateAsync(adminRole);
                adminRoleInDb = await _roleManager.FindByNameAsync(ApplicationConstans.RoleConstants.AdministratorRole);
            }

            var superUser = new ApplicationUser
            {               
                FirstName = "Mateusz",
                LastName = "Jóźwik",
                Email = "jmate220@gmail.com",
                UserName = "Mateusz",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                CreatedOn = DateTime.Now,
                IsActive = true
            };
            var superUserInDb = await _userManager.FindByEmailAsync(superUser.Email);
            if (superUserInDb == null)
            {
                await _userManager.CreateAsync(superUser, ApplicationConstans.UserConstants.DefaultPassword);
                var result = await _userManager.AddToRoleAsync(superUser, ApplicationConstans.RoleConstants.AdministratorRole);
            }
            foreach (var permission in Permissions.GetRegisteredPermissions())
            {
                await _roleManager.AddPermissionClaim(adminRoleInDb, permission);
            }
        }).GetAwaiter().GetResult();
    }

    private void AddBasicUser()
    {
        Task.Run(async () =>
        {
            var basicRole = new ApplicationRole()
            {
                Name = ApplicationConstans.RoleConstants.BasicRole,
                Description= "Basic role with default permissions"
            };
            var basicRoleInDb = await _roleManager.FindByNameAsync(ApplicationConstans.RoleConstants.BasicRole);
            if (basicRoleInDb == null)
            {
                await _roleManager.CreateAsync(basicRole);
                basicRoleInDb = await _roleManager.FindByNameAsync(ApplicationConstans.RoleConstants.BasicRole);
            }

            var basicUser = new ApplicationUser
            {                
                FirstName = "Rafał",
                LastName = "Breński",
                Email = "rafal.brenski@gmail.com",
                UserName = "Konto_Test",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                CreatedOn = DateTime.Now,
                IsActive = true
            };
            var basicUserInDb = await _userManager.FindByEmailAsync(basicUser.Email);
            if (basicUserInDb == null)
            {
                await _userManager.CreateAsync(basicUser, ApplicationConstans.UserConstants.DefaultPassword);
                await _userManager.AddToRoleAsync(basicUser, ApplicationConstans.RoleConstants.BasicRole);                
            }
        }).GetAwaiter().GetResult();
    }
}
