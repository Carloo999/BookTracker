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
    private readonly IWebHostEnvironment _hostEnvironment;
    
    public ApplicationUserManager(UserManager<ApplicationUser> userManager, ApplicationDbContext db, IWebHostEnvironment hostEnvironment)
    {
        _userManager = userManager;
        _db = db;
        _hostEnvironment = hostEnvironment;
    }

    private async Task<IdentityResult> AddUserToDb(ApplicationUser user, string password, byte[] profilePicture)
    {
        var bookList = new BookList();
        user.BookList = bookList;
        user.BookListId = user.BookList.Id;
        user.ProfilePicture = profilePicture;
        
        return await _userManager.CreateAsync(user, password);
    }

    public async Task<IdentityResult> CreateDefaultUser(ApplicationUser user, string password)
    {
        var defaultProfilePicture = await GetDefaultImage();
        
        await _userManager.AddToRoleAsync(user, Roles.User.ToString());
        return await AddUserToDb(user, password, defaultProfilePicture);
    }
    
    private async Task<byte[]> GetDefaultImage()
    {
        var imageFilePath = Path.Combine(_hostEnvironment.WebRootPath, "Assets", "defaultProfilePicture.png");
        
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

    public async Task<IdentityResult> DeleteUser(ApplicationUser user)
    {
        IdentityResult result = await _userManager.DeleteAsync(user);

        if (!result.Succeeded) return result;
        
        var userBooks = GetUserBooks(user);
        foreach (Book book in userBooks)
        {
            _db.Books.Remove(book);
        }

        _db.BookLists.Remove(GetUserBookList(user));
            
        await _db.SaveChangesAsync();

        return result;
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

    public async Task<IdentityResult> ChangePrivacySetting(ApplicationUser user, PrivacyStatus status)
    {
        user.PrivacyStatus = status;
        return await _userManager.UpdateAsync(user);
    }

    public async Task<(bool, bool)> GetIsAuthorized(ApplicationUser owner, ApplicationUser? currentUser)
    {
        PrivacyStatus status = owner.PrivacyStatus;
        var isDisplayed = false;
        var isEditable = false;

        if (status == PrivacyStatus.Public)
        {
            isDisplayed = true;
        }
        if (currentUser is null) return (isDisplayed, false);
        
        if (owner.Equals(currentUser))
        {
            isDisplayed = true;
            isEditable = true;
        }
        else
        {
            var roles = await GetUserRole(currentUser);

            if (!roles.Contains(Roles.Admin.ToString()) && !roles.Contains(Roles.Owner.ToString()))
                return (isDisplayed, isEditable);
            isDisplayed = true;
            isEditable = true;
        }

        return (isDisplayed, isEditable);
    }

    public List<ApplicationUser> GetUsers()
    {
        return _userManager.Users.ToList();
    }
}