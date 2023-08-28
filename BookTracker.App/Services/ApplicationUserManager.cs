using System.Security.Claims;
using BookTracker.App.Data;
using BookTracker.App.Enums;
using BookTracker.App.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;

namespace BookTracker.App.Services;

public class ApplicationUserManager : IApplicationUserManager
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _db;
    
    public ApplicationUserManager(UserManager<ApplicationUser> userManager, ApplicationDbContext db)
    {
        _userManager = userManager;
        _db = db;
    }

    private async Task AddUserToDb(ApplicationUser user, string password, byte[] profilePicture)
    {
        var newBl = new BookList();
        await _db.BookLists.AddAsync(newBl);
        user.BookList = newBl;
        user.BookListId = newBl.Id;
        user.ProfilePicture = new byte[1];
        await _userManager.CreateAsync(user, password);
    }

    public async Task CreateDefaultUser(ApplicationUser user, string password)
    {
        var defaultProfilePicture = GetDefaultImage();
        
        await _userManager.AddToRoleAsync(user, Roles.User.ToString());
        await AddUserToDb(user, password, defaultProfilePicture.Result);
    }
    
    private async Task<byte[]> GetDefaultImage()
    {
        var imageFilePath = "/Assets/defaultProfilePicture.png";
        
        if (!File.Exists(imageFilePath))
            throw new FileNotFoundException("Image file not found.");

        await using var fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
        await using var memoryStream = new MemoryStream();
        await fileStream.CopyToAsync(memoryStream);
        return memoryStream.ToArray();
    }
    
    public async Task CreateUser(ApplicationUser user, string password, List<Roles> roles, byte[] profilePicture)
    {
        foreach (Roles role in roles)
        {
            await _userManager.AddToRoleAsync(user, role.ToString());
        }
        await AddUserToDb(user, password, profilePicture);
    }

    public async Task DeleteUser(ApplicationUser user)
    {
        var userBooks = GetUserBooks(user);
        foreach (Book book in userBooks)
        {
            _db.Books.Remove(book);
        }

        _db.BookLists.Remove(GetUserBookList(user));
        await _db.SaveChangesAsync();
        await _userManager.DeleteAsync(user);
    }

    public BookList GetUserBookList(ApplicationUser user)
    {
        var b = _db.BookLists.Where(bl => user.BookListId == bl.Id);
        return b.First();
    }
    
    public List<Book> GetUserBooks(ApplicationUser user)
    {
        return _db.Books.Where(b => b.BookListId == user.BookListId).ToList();
    }

    public async Task<ApplicationUser?> GetUserByAuthState(AuthenticationState authState)
    {
        ClaimsPrincipal claims = authState.User;
        ApplicationUser? user = await _userManager.GetUserAsync(claims);
        return user;
    }

    public async Task<IList<string>> GetUserRole(ApplicationUser user)
    {
        return await _userManager.GetRolesAsync(user);
    }
}