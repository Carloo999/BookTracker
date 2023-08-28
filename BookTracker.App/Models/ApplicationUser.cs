﻿using System.ComponentModel.DataAnnotations.Schema;
using BookTracker.App.Enums;
using Microsoft.AspNetCore.Identity;

namespace BookTracker.App.Models;

public class ApplicationUser : IdentityUser
{
    public int UsernameChangeLimit { get; set; } = 2;
    public byte[] ProfilePicture { get; set; }
    
    public PrivacyStatus PrivacyStatus { get; set; }

    [ForeignKey("BookList")]
    public int BookListId { get; set; }
    public BookList BookList { get; set; }
}