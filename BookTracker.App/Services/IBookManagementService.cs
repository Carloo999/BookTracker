using BookTracker.App.Models;

namespace BookTracker.App.Services;

public interface IBookManagementService
{
    Task AddNewBook(Book book, ApplicationUser user);
    Task SaveBook();
    Task<ICollection<Book>> GetUserBooks(ApplicationUser user);
}