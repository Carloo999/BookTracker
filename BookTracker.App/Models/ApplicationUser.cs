using System.ComponentModel.DataAnnotations.Schema;
using BookTracker.Core.Models;
using Microsoft.AspNetCore.Identity;

namespace BookTracker.App.Models;

public class ApplicationUser : IdentityUser
{
    public int UsernameChangeLimit { get; set; } = 2;
    public byte[] ProfilePicture { get; set; }
    public bool PrivacyStatus { get; set; }

    [ForeignKey("BookList")]
    public int BookListId { get; set; }
    public BookList BookList { get; set; }
}