using System.ComponentModel.DataAnnotations.Schema;

namespace BookTracker.Core.Models;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public int Rating { get; set; }
    public int Pages { get; set; }
    public int PagesRead { get; set; }
    
    [ForeignKey("BookList")]
    public int BookListId { get; set; }
    public BookList BookList { get; set; }
}