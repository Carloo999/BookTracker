using BookTracker.App.Data;
using BookTracker.App.Models;
using Microsoft.EntityFrameworkCore;

namespace BookTracker.App.Services;

public class BookManagementService : IBookManagementService
{
    private ApplicationDbContext _dbContext;
    
    public BookManagementService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task AddNewBook(Book book, ApplicationUser user)
    {
        book.BookListId = user.BookListId;
        _dbContext.Books.Add(book);
        await SaveBook();

        var books = await GetUserBooks(user);
    }
    
    public async Task SaveBook()
    {
        await _dbContext.SaveChangesAsync();
    }

    public async Task<ICollection<Book>> GetUserBooks(ApplicationUser user)
    {
        var blId = user.BookListId;

         return await _dbContext.Books
            .Where(b => b.BookListId == blId)
            .ToListAsync();
    }

    public async Task DeleteBook(Book book)
    {
        _dbContext.Books.Remove(book);
        await SaveBook();
    } 
}