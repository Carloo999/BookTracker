using BookTracker.App.Enums;
using BookTracker.App.Models;
using Microsoft.AspNetCore.Components.Authorization;

namespace BookTracker.App.Services;

public interface IApplicationUserManager
{
    Task CreateUser(ApplicationUser user, string password, List<Roles> roles, byte[] profilePicture);

    Task DeleteUser(ApplicationUser user);

    Task<ApplicationUser?> GetUserByAuthState(AuthenticationState authenticationState);
    
    Task<IList<string>> GetUserRole(ApplicationUser user);

    Task CreateDefaultUser(ApplicationUser user, string password);
}