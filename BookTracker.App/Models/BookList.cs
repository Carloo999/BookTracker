namespace BookTracker.App.Models;

public class BookList
{
    public int Id { get; set; }
    public ICollection<Book> Books { get; set; }
    
    public ApplicationUser User { get; set; }
}