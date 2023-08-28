using BookTracker.App.Enums;
using BookTracker.App.Models;
using BookTracker.App.Services;
using Microsoft.AspNetCore.Identity;

namespace BookTracker.App.Data;

public class ContextSeed
{
    
    public static async Task SeedRolesAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        //Seed Roles
        await roleManager.CreateAsync(new IdentityRole(Enums.Roles.Owner.ToString()));
        await roleManager.CreateAsync(new IdentityRole(Enums.Roles.Admin.ToString()));
        await roleManager.CreateAsync(new IdentityRole(Enums.Roles.User.ToString()));
    }
    
    public static async Task SeedOwnerAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IApplicationUserManager appUserManager)
    {
        //Seed Default User
        var defaultUser = new ApplicationUser 
        { 
            UserName = "owner", 
            Email = "owner@gmail.com",
            EmailConfirmed = true, 
            PhoneNumberConfirmed = true,
            ProfilePicture = new byte[1]
        };
        if (userManager.Users.All(u => u.Id != defaultUser.Id))
        {
            ApplicationUser? user = await userManager.FindByEmailAsync(defaultUser.Email);
            if(user==null)
            {
                //TODO profile picture 
                await appUserManager.CreateUser(defaultUser, "Test123!.", new List<Roles>() { Roles.User , Roles.Admin, Roles.Owner}, new byte[1]);
            }
               
        }
    }
}