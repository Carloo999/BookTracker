using System.ComponentModel.DataAnnotations.Schema;
using BookTracker.App.Models;

namespace BookTracker.Core.Models;

public class BookList
{
    public int Id { get; set; }
    public ICollection<Book> Books { get; set; }
    
    public ApplicationUser User { get; set; }
}